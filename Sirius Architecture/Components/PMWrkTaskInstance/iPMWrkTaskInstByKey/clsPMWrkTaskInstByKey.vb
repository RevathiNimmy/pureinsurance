Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Globalization
Imports System.Windows.Forms
Imports SharedFiles
Friend NotInheritable Class Interface_Renamed 
	  Implements IDisposable
' ***************************************************************** '
	' Class Name: Interface
	'
	' Date: 26/10/1998
	'
	' Description: Handles the display of the Form etc.
	'
	' Edit History:
	'
	' ***************************************************************** '
	
	
	' Constant for the functions to identify which class this is.
Private Const ACClass As String = "Interface" 
	
	' PUBLIC Data Members (Begin)
	' PUBLIC Data Members (End)
	
	
	' PRIVATE Data Members (Begin)
	Private m_oObjectManager As bObjectManager.ObjectManager
	Private m_sUsername As String = ""
	Private m_iUserID As Integer
	
	' Main Form
	Private WithEvents m_frmInterface As frmInterface
	
	' Scheduled Task Collection
	Private m_oSchedTasks As ScheduledTasks
	

	Private m_oBusiness As bPMWrkManager.FormClass
	'Private m_oBusiness As bPMWrkManager.FormClass
	
	' The currently selected Scheduled Task
	Private m_sScheduledTaskKey As String = ""
	
	' CallingAppName
	Private m_sCallingAppName As String = ""
	' PMAuthorityLevel
	Private m_lPMAuthorityLevel As Integer
	' Status
	Private m_lStatus As Integer
	' Task
	Private m_iTask As Integer
	' Navigate
	Private m_lNavigate As Integer
	' ProcessMode
	Private m_lProcessMode As Integer
	' TransactionType
	Private m_sTransactionType As String = ""
	' EffectiveDate
	Private m_dtEffectiveDate As Date
	
	Private m_lReturn As Integer
	
	Private m_sKeyName As String = ""
	
	Private m_sKeyValue As String = ""
	
	' PRIVATE Data Members (End)
	
	' PUBLIC Property Procedures (Begin)
	Public ReadOnly Property Username() As String
		Get
			Return m_sUsername.Trim()
		End Get
	End Property
	Public ReadOnly Property UserID() As Integer
		Get
			Return m_iUserID
		End Get
	End Property
	
	Public Property Status() As Integer
		Get
			Return m_lStatus
		End Get
		Set(ByVal Value As Integer)
			m_lStatus = Value
		End Set
	End Property
	
	Public Property Task() As Integer
		Get
			Return m_iTask
		End Get
		Set(ByVal Value As Integer)
			m_iTask = Value
		End Set
	End Property
	
	Public Property Navigate() As Integer
		Get
			Return m_lNavigate
		End Get
		Set(ByVal Value As Integer)
			m_lNavigate = Value
		End Set
	End Property
	
	Public Property ProcessMode() As Integer
		Get
			Return m_lProcessMode
		End Get
		Set(ByVal Value As Integer)
			m_lProcessMode = Value
		End Set
	End Property
	
	Public Property TransactionType() As String
		Get
			Return m_sTransactionType
		End Get
		Set(ByVal Value As String)
			m_sTransactionType = Value
		End Set
	End Property
	
	Public Property EffectiveDate() As Date
		Get
			Return m_dtEffectiveDate
		End Get
		Set(ByVal Value As Date)
			m_dtEffectiveDate = Value
		End Set
	End Property
	
	' PUBLIC Property Procedures (End)
	' PRIVATE Property Procedures (Begin)
	Private Property ScheduledTaskKey() As String
		Get
			Return m_sScheduledTaskKey.Trim()
		End Get
		Set(ByVal Value As String)
			m_sScheduledTaskKey = Value.Trim()
		End Set
	End Property
	
	Public Property CallingAppName() As String
		Get
			Return m_sCallingAppName
		End Get
		Set(ByVal Value As String)
			m_sCallingAppName = Value
		End Set
	End Property
	
	Public Property PMAuthorityLevel() As Integer
		Get
			Return m_lPMAuthorityLevel
		End Get
		Set(ByVal Value As Integer)
			m_lPMAuthorityLevel = Value
		End Set
	End Property
	
	' PRIVATE Property Procedures (End)
	
	
	' PUBLIC Methods (Begin)
	
	' ***************************************************************** '
	' Name: Initialise (Standard Method)
	'
	' Description: Entry point for any initialisation code for this
	'              object.
	'
	' ***************************************************************** '
	Public Function Initialise() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			m_oObjectManager = New bObjectManager.ObjectManager()
			
			m_lReturn = m_oObjectManager.Initialise(ACApp)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return m_lReturn
			End If
			
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
			
			With m_oObjectManager
				m_sUsername = .UserName
				m_iUserID = .UserID
			End With
			
			' Get the Business
			Dim temp_m_oBusiness As Object
			m_lReturn = m_oObjectManager.GetInstance(temp_m_oBusiness, "bPMWrkManager.FormClass", vinstancemanager:=gPMConstants.PMGetViaClientManager)
			m_oBusiness = temp_m_oBusiness
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				m_oBusiness = Nothing
				m_oObjectManager = Nothing
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				Return m_lReturn
			End If
			
			' Create the Form
			m_frmInterface = New frmInterface()
			' Set the Form Parent
			m_frmInterface.Parent_Renamed = Me
			
			' Create New Scheduled Tasks Collection
			m_oSchedTasks = New ScheduledTasks()
			m_lReturn = CType(m_oSchedTasks, SSP.S4I.Interfaces.ILocalInterface).Initialise()
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				m_oBusiness = Nothing
				m_oObjectManager = Nothing
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				Return m_lReturn
			End If
			
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			DisplayErrorMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			
			m_oBusiness = Nothing
			m_oObjectManager = Nothing
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: Terminate (Standard Method)
	'
	' Description: Entry point for any termination code for this
	'              object.
	'
	' ***************************************************************** '
 Private disposedValue As Boolean
	Public Sub Dispose() Implements IDisposable.Dispose
		Dispose(True)
		GC.SuppressFinalize(Me)
	End Sub


	Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            Me.disposedValue = True
            If disposing Then
                If m_oSchedTasks IsNot Nothing Then
                    m_oSchedTasks.Dispose()
                    m_oSchedTasks = Nothing
                End If
                If m_oBusiness IsNot Nothing Then
                    m_oBusiness.Dispose()
                    m_oBusiness = Nothing
                End If

                If Not (m_frmInterface Is Nothing) Then
                    ' Terminate Form
                    m_frmInterface.Parent_Renamed = Nothing
                    m_frmInterface.Close()
                    m_frmInterface = Nothing
                End If
            End If
        End If
		Me.disposedValue = True
	End Sub

	
	' ***************************************************************** '
	' Name: Start
	'
	' Description: Start Work Manager.
	'
	'
	' ***************************************************************** '
	Public Function Start() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
			
			' Get the Registry Settings
			GetRegistrySettings()
			
			' Setup the Form
			m_lReturn = SetFormForDisplay()
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Show the Form
			m_frmInterface.ShowDialog()
			
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			DisplayErrorMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="StartFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: GetUserAuthority
	'
	' Description: Returns whether the User is a Sys Admin or Supervisor
	'              or Normal User.
	' ***************************************************************** '
	Public Function GetUserAuthority(ByRef r_lPMAuthorityLevel As Integer, Optional ByVal v_lUserGroupID As Integer = 0) As Integer
		Dim result As Integer = 0
        Dim oBusiness As bPMWrkManager.FormClass
		Static bIsAdministrator As Boolean
		Static vSupervisedGroups As Object
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Default the Authority to User
			r_lPMAuthorityLevel = gPMConstants.PMEAuthorityLevel.pmeALUser
			
			' If we have NOT previously got the User Authority details
			' then get them.

			If Object.Equals(vSupervisedGroups, Nothing) Then
				
				' Get the Business
				Dim temp_oBusiness As Object
				m_lReturn = m_oObjectManager.GetInstance(temp_oBusiness, "bPMWrkManager.FormClass", vinstancemanager:=gPMConstants.PMGetViaClientManager)
				oBusiness = temp_oBusiness
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					Return m_lReturn
				End If
				

				m_lReturn = oBusiness.GetUserAuthority(r_bIsAdministrator:=bIsAdministrator, r_vSupervisedGroups:=vSupervisedGroups)
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    oBusiness.Dispose()
					oBusiness = Nothing
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
				

                oBusiness.Dispose()
				oBusiness = Nothing
				
			End If
			
			' Is the User an Administrator
			If bIsAdministrator Then
				' Yes, so set the Level and exit.
				' Note: There is no need to check if they are a Group
				'       Supervisor as SysAdmin is a higher level authority.
				r_lPMAuthorityLevel = gPMConstants.PMEAuthorityLevel.pmeALSysAdmin
				Return result
			End If
			
			' Has A GroupID been supplied ?
			If v_lUserGroupID > 0 Then
				' Do they supervise any Groups ?
				If Information.IsArray(vSupervisedGroups) Then
					' Yes, so check to see if they Supervise the Group supplied

					For lRow As Integer = vSupervisedGroups.GetLowerBound(1) To vSupervisedGroups.GetUpperBound(1)
						' If the GroupID's match

                        If (v_lUserGroupID) = CInt(vSupervisedGroups(0, lRow)) Then
                            ' Set Authority Level to Supervisor
                            r_lPMAuthorityLevel = gPMConstants.PMEAuthorityLevel.pmeALSupervisor
                            Exit For
                        End If
					Next lRow
				End If
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			DisplayErrorMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetUserAuthorityFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUserAuthority", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	'
	' Name: SetKeys
	'
	' Description:
	'
	' History: 10/05/2000 DAK - Created.
	'
	' ***************************************************************** '
Public Function SetKeys(ByRef vKeyArray( , ) As Object ) As Integer
		Dim result As Integer = 0

		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			m_sKeyName = ""
			m_sKeyValue = ""
			
			' Check we have a valid array.
			If Not Information.IsArray(vKeyArray) Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			

			m_sKeyName = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, vKeyArray.GetUpperBound(1)))

			m_sKeyValue = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, vKeyArray.GetUpperBound(1)))
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	'
	' Name: GetKeys
	'
	' Description:
	'
	' History: 10/05/2000 DAK - Created.
	'
	' ***************************************************************** '
Public Function GetKeys(ByRef vKeyArray(,) As Object ) As Integer
		
		Dim result As Integer = 0
		Try 
			
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: SetProcessModes (Standard Method)
	'
	' Description: Set the optional process modes.
	'
	' ***************************************************************** '
	Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Assign the process modes to the property members.
			

			If Not Information.IsNothing(vTask) Then

				m_iTask = CInt(vTask)
			End If
			

			If Not Information.IsNothing(vNavigate) Then

				m_lNavigate = CInt(vNavigate)
			End If
			

			If Not Information.IsNothing(vProcessMode) Then

				m_lProcessMode = CInt(vProcessMode)
			End If
			

			If Not Information.IsNothing(vTransactionType) Then

				m_sTransactionType = CStr(vTransactionType)
			End If
			

			If Not Information.IsNothing(vEffectiveDate) Then

				m_dtEffectiveDate = CDate(vEffectiveDate)
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' PUBLIC Methods (End)
	
	
	' PRIVATE Methods (Begin)
	
	' ***************************************************************** '
	' Name: SetFormForDisplay
	'
	' Description: Gets the Form ready for display.
	'
	' ***************************************************************** '
	Private Function SetFormForDisplay() As Integer
		Dim result As Integer = 0
		Dim lPMAuthorityLevel As Integer
		'DAK110100

		 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			With m_frmInterface
				' Populate the Task Status
				.cboTaskStatus.Items.Add(ACListTaskTypeAllButComplete)
				.cboTaskStatus.Items.Add(ACListTaskTypeAll)
				.cboTaskStatus.Items.Add(ACListTaskTypeNew)
				.cboTaskStatus.Items.Add(ACListTaskTypeInProgress)
				.cboTaskStatus.Items.Add(ACListTaskTypeInComplete)
				.cboTaskStatus.Items.Add(ACListTaskTypeComplete)
				.cboTaskStatus.SelectedIndex = 0
				
				' Populate the Date Range
				.cboDateRange.Items.Add(ACDateRangeDescAll)

				VB6.SetItemData(.cboDateRange, "NewIndex", ACDateRangeIndexAll)
				.cboDateRange.Items.Add(ACDateRangeDescToday)

				VB6.SetItemData(.cboDateRange, "NewIndex", ACDateRangeIndexToday)
				.cboDateRange.Items.Add(ACDateRangeDescNext1)

				VB6.SetItemData(.cboDateRange, "NewIndex", ACDateRangeIndexNext1)
				.cboDateRange.Items.Add(ACDateRangeDescNext2)

				VB6.SetItemData(.cboDateRange, "NewIndex", ACDateRangeIndexNext2)
				.cboDateRange.Items.Add(ACDateRangeDescNext3)

				VB6.SetItemData(.cboDateRange, "NewIndex", ACDateRangeIndexNext3)
				.cboDateRange.Items.Add(ACDateRangeDescNext4)

				VB6.SetItemData(.cboDateRange, "NewIndex", ACDateRangeIndexNext4)
				.cboDateRange.Items.Add(ACDateRangeDescNext5)

				VB6.SetItemData(.cboDateRange, "NewIndex", ACDateRangeIndexNext5)
				.cboDateRange.Items.Add(ACDateRangeDescNext6)

				VB6.SetItemData(.cboDateRange, "NewIndex", ACDateRangeIndexNext6)
				.cboDateRange.Items.Add(ACDateRangeDescNext7)

				VB6.SetItemData(.cboDateRange, "NewIndex", ACDateRangeIndexNext7)
				.cboDateRange.Items.Add(ACDateRangeDescNext14)

				VB6.SetItemData(.cboDateRange, "NewIndex", ACDateRangeIndexNext14)
				.cboDateRange.Items.Add(ACDateRangeDescNext28)

				VB6.SetItemData(.cboDateRange, "NewIndex", ACDateRangeIndexNext28)
				' Default is All Dates
				.cboDateRange.SelectedIndex = ACDateRangeIndexAll
				
				'DAK040100
				' Enable the Timer
				'        .tmrSystemTasks.Enabled = True
				'        .tmrSystemTasks.Interval = ACSystemTasksTimerInterval
				If .IsAutoRefresh Then
					.tmrSystemTasks.Enabled = True
					If ACSystemTasksTimerInterval = 0 Then
						.tmrSystemTasks.Enabled = False
					Else
						.tmrSystemTasks.Interval = ACSystemTasksTimerInterval
						.tmrSystemTasks.Enabled = True
					End If
				Else
					.tmrSystemTasks.Enabled = False
				End If
				
				' Get the User Authority
				m_lReturn = GetUserAuthority(r_lPMAuthorityLevel:=lPMAuthorityLevel)
				
				.KeyName = m_sKeyName
				.KeyValue = m_sKeyValue
				
			End With
			
			' Get the Form Ready for initial display
			m_lReturn = m_frmInterface.SetForDisplay()
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Populate Scheduled Tasks
			m_lReturn = PopulateSchedTasks(v_bForceRefresh:=True)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			Return result
		
	End Function
	
	' ***************************************************************** '
	' Name: PopulateSchedTasks
	'
	' Description: Populate the Scheduled Tasks List View
	'
	'
	' ***************************************************************** '
	Private Function PopulateSchedTasks(ByVal v_bForceRefresh As Boolean) As Integer
		
		Dim result As Integer = 0
        'Developer Guide No. 179
        Dim vSchedTaskArray As Object

        Dim lCurrentTaskStatus As gPMConstants.PMEWrkManTaskStatus
        Dim lCurrentPMUserGroupID, lCurrentUserID As Integer
        Dim dtEndDate As Date
        Dim bCurrentOmitCompleted As Boolean
        Static lPreviousTaskStatus As gPMConstants.PMEWrkManTaskStatus
        Static lPreviousPMUserGroupID, lPreviousUserID As Integer
        Static dtPreviousEndDate As Date
        Static bPreviousOmitCompleted As Boolean
        Static sPreviousShowSystem As String = ""


        

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Default the Current Selection Criteria
            lCurrentTaskStatus = -1
            lCurrentPMUserGroupID = -1
            lCurrentUserID = -1
            dtEndDate = #12/29/1899#
            bCurrentOmitCompleted = False

            ' Set the Specific Selection Criteria
            Select Case VB6.GetItemString(m_frmInterface.cboTaskStatus, m_frmInterface.cboTaskStatus.SelectedIndex)
                ' All Tasks
                Case ACListTaskTypeAll

                    ' New Only
                Case ACListTaskTypeNew
                    lCurrentTaskStatus = gPMConstants.PMEWrkManTaskStatus.pmeWMTSNew

                Case ACListTaskTypeInProgress
                    lCurrentTaskStatus = gPMConstants.PMEWrkManTaskStatus.pmeWMTSInProgress

                Case ACListTaskTypeComplete
                    lCurrentTaskStatus = gPMConstants.PMEWrkManTaskStatus.pmeWMTSComplete

                Case ACListTaskTypeInComplete
                    lCurrentTaskStatus = gPMConstants.PMEWrkManTaskStatus.pmeWMTSIncomplete

                Case ACListTaskTypeAllButComplete
                    bCurrentOmitCompleted = True
            End Select

            lCurrentPMUserGroupID = m_frmInterface.cboUserGroup.UserGroupID
            If lCurrentPMUserGroupID < 1 Then
                lCurrentPMUserGroupID = -1
            End If
            lCurrentUserID = m_frmInterface.cboUser.UserID
            If lCurrentUserID < 1 Then
                lCurrentUserID = -1
            End If

            ' Default the End Date to End of Today
            dtEndDate = DateTime.Today.AddDays(DateAndTime.TimeSerial(23, 59, 59).ToOADate())

            ' Work out the Date Range End Date
            Select Case (VB6.GetItemData(m_frmInterface.cboDateRange, m_frmInterface.cboDateRange.SelectedIndex))
                Case ACDateRangeIndexAll
                    ' Set end date to include all dates
                    dtEndDate = #12/29/1899#
                Case ACDateRangeIndexToday
                    ' Already Defaulted to this
                Case ACDateRangeIndexNext1
                    ' Add 1 Day
                    dtEndDate = dtEndDate.AddDays(1)
                Case ACDateRangeIndexNext2
                    ' Add 2 Day
                    dtEndDate = dtEndDate.AddDays(2)
                Case ACDateRangeIndexNext3
                    ' Add 3 Day
                    dtEndDate = dtEndDate.AddDays(3)
                Case ACDateRangeIndexNext4
                    ' Add 4 Day
                    dtEndDate = dtEndDate.AddDays(4)
                Case ACDateRangeIndexNext5
                    ' Add 5 Day
                    dtEndDate = dtEndDate.AddDays(5)
                Case ACDateRangeIndexNext6
                    ' Add 6 Day
                    dtEndDate = dtEndDate.AddDays(6)
                Case ACDateRangeIndexNext7
                    ' Add 7 Day
                    dtEndDate = dtEndDate.AddDays(7)
                Case ACDateRangeIndexNext14
                    ' Add 14 Day
                    dtEndDate = dtEndDate.AddDays(14)
                Case ACDateRangeIndexNext28
                    ' Add 28 Day
                    dtEndDate = dtEndDate.AddDays(28)
                Case Else
                    ' Set end date to include all dates
                    dtEndDate = #12/29/1899#
            End Select

            ' If we are NOT Forcing a Refresh AND
            ' we have we already filled the list with this Selection then EXIT
            If (Not v_bForceRefresh) And (lPreviousTaskStatus = lCurrentTaskStatus) And (lPreviousPMUserGroupID = lCurrentPMUserGroupID) And (lPreviousUserID = lCurrentUserID) And (dtPreviousEndDate = dtEndDate) And (bPreviousOmitCompleted = bCurrentOmitCompleted) Then
                Return result
            End If

            m_frmInterface.UpdateStatusBar(v_vActivity:=ACStatusActSearching)

            ' Clear the any existing tasks
            m_frmInterface.lstScheduledTasks.Items.Clear()
            ' Turn Sorting Off while we add the New Scheduled Tasks
            'TODO
            'm_frmInterface.listViewHelper1.SetSorted(m_frmInterface.lstScheduledTasks, False)
            ' Clear the Collection
            m_oSchedTasks.Clear()

            ' Get the Scheduled Tasks from business

            m_lReturn = m_oBusiness.GetTaskInstByKey(v_sKeyName:=m_sKeyName, v_sKeyValue:=m_sKeyValue, r_vTaskArray:=vSchedTaskArray, v_lTaskStatus:=lCurrentTaskStatus, v_lPmuserGroupID:=lCurrentPMUserGroupID, v_iUserID:=lCurrentUserID, v_dtDueDateLimit:=dtEndDate, v_bOmitCompleted:=bCurrentOmitCompleted)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                DisplayErrorMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the Scheduled Tasks.", vApp:=ACApp, vClass:=ACClass)
                Return result
            End If

            ' Add the Scheduled Tasks
            m_lReturn = AddScheduledTasks(r_vSchedTaskArray:=vSchedTaskArray, v_bDisplayOnForm:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                DisplayErrorMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to populate the Scheduled Tasks.", vApp:=ACApp, vClass:=ACClass)
                Return result
            End If

            ' Turn Sorting Back On & Refresh
            'TODO
            m_frmInterface.lstScheduledTasks.Refresh()
            m_frmInterface.lstScheduledTasks.Sort()

            ' All OK so save the selection criteria
            lPreviousTaskStatus = lCurrentTaskStatus
            lPreviousPMUserGroupID = lCurrentPMUserGroupID
            lPreviousUserID = lCurrentUserID
            dtPreviousEndDate = dtEndDate
            bPreviousOmitCompleted = bCurrentOmitCompleted

            ' Release any Memory used by the Arrays
        'vSchedTaskArray = ""
            vSchedTaskArray = Nothing

            m_frmInterface.UpdateStatusBar(v_vActivity:="")

            Return result

	End Function
	
	' ***************************************************************** '
	' Name: AddScheduledTasks
	'
	' Description: Adds the Scheduled Tasks from the Array.
	'
	'
	' ***************************************************************** '
	Private Function AddScheduledTasks(ByRef r_vSchedTaskArray( ,  ) As Object, ByVal v_bDisplayOnForm As Boolean) As Integer
		
		Dim result As Integer = 0
		Dim iIsUrgent, iTaskType, iIsSystem, iTaskStatus As Integer
		Dim lTaskInstanceCnt As Integer
		Dim dtTaskDueDate As Date
		Dim sCustomer, sDescription As String
		Dim lUserGroupID As Integer
		Dim vUserID As String = ""
		Dim vPMNavProcessID As String = ""
		Dim lPMNavprocessID As Integer
		Dim sComponentObjectName, sComponentClassName As String
		Dim lDisplayIcon As Integer
		Dim iIsViewOnlyTask As Integer
		Dim sLinkedObjectName, sLinkedClassName, sLinkedCaption As String
		'DAK141299
		Dim iIsVisible As Integer
		
		
		 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' If there is no Summary to add, just exit.
			If Not Information.IsArray(r_vSchedTaskArray) Then
				Return result
			End If
			
			' Add Each Row in the Array to the List
			For lRow As Integer = r_vSchedTaskArray.GetLowerBound(1) To r_vSchedTaskArray.GetUpperBound(1)
				
				' Get the values from the Array

				iIsUrgent = CInt(r_vSchedTaskArray(gPMConstants.PMEACSchedTaskCol.ACSchedTaskUrgentCol, lRow))

				iTaskType = CInt(r_vSchedTaskArray(gPMConstants.PMEACSchedTaskCol.ACSchedTaskTypeCol, lRow))

				iIsSystem = CInt(r_vSchedTaskArray(gPMConstants.PMEACSchedTaskCol.ACSchedTaskIsSystemCol, lRow))

				iTaskStatus = CInt(r_vSchedTaskArray(gPMConstants.PMEACSchedTaskCol.ACSchedTaskStatusCol, lRow))

				lTaskInstanceCnt = CInt(r_vSchedTaskArray(gPMConstants.PMEACSchedTaskCol.ACSchedTaskInstanceCntCol, lRow))

				dtTaskDueDate = CDate(r_vSchedTaskArray(gPMConstants.PMEACSchedTaskCol.ACSchedTaskDueDateCol, lRow))

				sCustomer = CStr(r_vSchedTaskArray(gPMConstants.PMEACSchedTaskCol.ACSchedTaskCustomerCol, lRow)).Trim()

				sDescription = CStr(r_vSchedTaskArray(gPMConstants.PMEACSchedTaskCol.ACSchedTaskDescriptionCol, lRow)).Trim()

				lUserGroupID = CInt(CStr(r_vSchedTaskArray(gPMConstants.PMEACSchedTaskCol.ACSchedTaskUserGroupIDCol, lRow)).Trim())

				vUserID = CStr(r_vSchedTaskArray(gPMConstants.PMEACSchedTaskCol.ACSchedTaskUserIDCol, lRow)).Trim()

				vPMNavProcessID = CStr(r_vSchedTaskArray(gPMConstants.PMEACSchedTaskCol.ACSchedTaskNavProcessIDCol, lRow))
				Dim dbNumericTemp As Double
				If Double.TryParse(vPMNavProcessID, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
					lPMNavprocessID = CInt(vPMNavProcessID)
				Else
					lPMNavprocessID = 0
				End If

				sComponentObjectName = CStr(r_vSchedTaskArray(gPMConstants.PMEACSchedTaskCol.ACSchedTaskObjectNameCol, lRow)).Trim()

				sComponentClassName = CStr(r_vSchedTaskArray(gPMConstants.PMEACSchedTaskCol.ACSchedTaskClassNameCol, lRow)).Trim()

				lDisplayIcon = CInt(r_vSchedTaskArray(gPMConstants.PMEACSchedTaskCol.ACSchedTaskDisplayIconCol, lRow))

				iIsViewOnlyTask = CInt(r_vSchedTaskArray(gPMConstants.PMEACSchedTaskCol.ACSchedTaskIsViewOnlyTaskCol, lRow))

				sLinkedObjectName = CStr(r_vSchedTaskArray(gPMConstants.PMEACSchedTaskCol.ACSchedTaskLinkedObjectNameCol, lRow)).Trim()

				sLinkedClassName = CStr(r_vSchedTaskArray(gPMConstants.PMEACSchedTaskCol.ACSchedTaskLinkedClassNameCol, lRow)).Trim()

				sLinkedCaption = CStr(r_vSchedTaskArray(gPMConstants.PMEACSchedTaskCol.ACSchedTaskLinkedCaptionCol, lRow)).Trim()
				'DAK141299

				iIsVisible = CInt(r_vSchedTaskArray(gPMConstants.PMEACSchedTaskCol.ACSchedTaskIsVisibleCol, lRow))
				
				' Add the Scheduled Task
				'DAK141299
				m_lReturn = AddScheduledTask(v_lPMWrkTaskInstanceCnt:=lTaskInstanceCnt, v_iIsUrgent:=iIsUrgent, v_iTaskStatus:=iTaskStatus, v_iTypeOfTask:=iTaskType, v_iIsSystem:=iIsSystem, v_dtTaskDueDate:=dtTaskDueDate, v_sCustomer:=sCustomer, v_sDescription:=sDescription, v_lUserGroupID:=lUserGroupID, v_vUserID:=CByte(vUserID), v_lNavProcessID:=lPMNavprocessID, v_sComponentObjectName:=sComponentObjectName, v_sComponentClassName:=sComponentClassName, v_lDisplayIcon:=lDisplayIcon, v_iIsViewOnlyTask:=iIsViewOnlyTask, v_sLinkedObjectName:=sLinkedObjectName, v_sLinkedClassName:=sLinkedClassName, v_sLinkedCaption:=sLinkedCaption, v_iIsVisible:=iIsVisible, v_bDisplayOnForm:=v_bDisplayOnForm)
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
				
			Next lRow
			
			Return result
		
	End Function
	
	' ***************************************************************** '
	' Name: AddScheduledTask
	'
	' Description: Adds Scheduled Task to the List View and the Collection
	'
	' ***************************************************************** '
	'DAK141299
	Private Function AddScheduledTask(ByVal v_lPMWrkTaskInstanceCnt As Integer, ByVal v_sCustomer As String, ByVal v_sDescription As String, ByVal v_dtTaskDueDate As Date, ByVal v_iIsUrgent As Integer, ByVal v_iTypeOfTask As Integer, ByVal v_iIsSystem As Integer, ByVal v_iTaskStatus As Integer, ByVal v_lUserGroupID As Integer, ByVal v_vUserID As Byte, ByVal v_lNavProcessID As Integer, ByVal v_sComponentObjectName As String, ByVal v_sComponentClassName As String, ByVal v_lDisplayIcon As Integer, ByVal v_iIsViewOnlyTask As Integer, ByVal v_sLinkedObjectName As String, ByVal v_sLinkedClassName As String, ByVal v_sLinkedCaption As String, ByVal v_iIsVisible As Integer, ByVal v_bDisplayOnForm As Boolean) As Integer
		
		Dim result As Integer = 0
		Dim sTaskTypeDesc, sTaskStatusDesc, sUserGroup, sUser, sKey As String
		
		 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Work out the Task Type desciption
			Select Case v_iTypeOfTask
				Case gPMConstants.PMEWrkManTaskType.pmeWMTTMemo
					sTaskTypeDesc = ACTaskTypeDescMemo
				Case gPMConstants.PMEWrkManTaskType.pmeWMTTSingleComponent
					sTaskTypeDesc = ACTaskTypeDescSingle
				Case gPMConstants.PMEWrkManTaskType.pmeWMTTNavigatorProcess
					sTaskTypeDesc = ACTaskTypeDescNavigator
			End Select
			
			If v_iIsSystem = gPMConstants.PMEReturnCode.PMTrue Then
				sTaskTypeDesc = ACTaskTypeDescSystem
			End If
			
			' Work Out the Task Status Description
			sTaskStatusDesc = TaskStatusDescription(v_iTaskStatus)
			
			' Get the User Group Caption
			sUserGroup = m_frmInterface.cboUserGroup.ItemUserGroupname(v_lUserGroupID)
			
			' Get the User Caption
			Dim dbNumericTemp As Double
			If Double.TryParse(CStr(v_vUserID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
				If v_vUserID < 1 Then
					sUser = ""
				Else
					sUser = m_frmInterface.cboAllUsers.ItemUsername(v_vUserID)
				End If
			Else
				sUser = ""
			End If
			
			'DAK141299
			m_lReturn = m_oSchedTasks.Add(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt, v_iIsUrgent:=v_iIsUrgent, v_iTaskStatus:=v_iTaskStatus, v_iTypeOfTask:=v_iTypeOfTask, v_iIsSystemTask:=v_iIsSystem, v_dtTaskDueDate:=v_dtTaskDueDate, v_sCustomer:=v_sCustomer, v_sDescription:=v_sDescription, v_lPmuserGroupID:=v_lUserGroupID, v_vUserID:=CStr(v_vUserID), v_sUserGroup:=sUserGroup, v_sUser:=sUser, v_lNavProcessID:=v_lNavProcessID, v_sComponentObjectName:=v_sComponentObjectName, v_sComponentClassName:=v_sComponentClassName, v_lDisplayIcon:=v_lDisplayIcon, v_iIsViewOnlyTask:=v_iIsViewOnlyTask, v_sLinkedObjectName:=v_sLinkedObjectName, v_sLinkedClassName:=v_sLinkedClassName, v_sLinkedCaption:=v_sLinkedCaption, v_iIsVisible:=v_iIsVisible, r_sKey:=sKey)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' If we are to Display this Task, then Add on Form
			'DAK141299
			If v_bDisplayOnForm And v_iIsVisible = gPMConstants.PMEReturnCode.PMTrue Then
				
				m_lReturn = m_frmInterface.AddScheduledTaskToList(v_sKey:=sKey, v_iIsUrgent:=v_iIsUrgent, v_sTaskStatusDesc:=sTaskStatusDesc, v_sTaskTypedesc:=sTaskTypeDesc, v_dtTaskDueDate:=v_dtTaskDueDate, v_sCustomer:=v_sCustomer, v_sDescription:=v_sDescription, v_sUserGroup:=sUserGroup, v_sUser:=sUser)
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
				
			End If
			
			Return result
		
	End Function
	
	' ***************************************************************** '
	' Name: UpdateScheduledTask
	'
	' Description: Updates the details for a Scheduled Task in the
	'              List View and Collection.
	'
	' ***************************************************************** '
	Private Sub UpdateScheduledTask(ByVal v_lPMWrkTaskInstanceCnt As Integer, Optional ByVal v_vCustomer As String = "", Optional ByVal v_vDescription As String = "", Optional ByVal v_vTaskDueDate As Date = #12/30/1899#, Optional ByVal v_vIsUrgent As Integer = 0, Optional ByVal v_vTaskStatus As gPMConstants.PMEWrkManTaskStatus = 0, Optional ByVal v_vUserGroupID As String = "", Optional ByVal v_vUserID As String = "", Optional ByVal v_vNavProcessID As Integer = 0, Optional ByVal v_vComponentObjectName As String = "", Optional ByVal v_vComponentClassName As String = "", Optional ByVal v_vDisplayIcon As Integer = 0, Optional ByVal v_vIsViewOnlyTask As Integer = 0, Optional ByVal v_vLinkedObjectName As String = "", Optional ByVal v_vLinkedClassName As String = "", Optional ByVal v_vLinkedCaption As String = "")
		
		Dim sScheduledTaskKey As String = ""
		Dim oSchedTask As iPMWrkTaskInstByKey.ScheduledTask
		Dim sStatusDesc, sUserGroup, sUser As String
		'DAK141299
		Dim iIsVisible As gPMConstants.PMEReturnCode
		
		
		 
			
			' Generate the Collection Key for this Task Instance
			sScheduledTaskKey = m_oSchedTasks.GenerateKey(v_lScheduledTaskCnt:=v_lPMWrkTaskInstanceCnt)
			
			' Get a Reference to the Scheduled Task
			oSchedTask = m_oSchedTasks.Item(sScheduledTaskKey)
			
			' Is it loaded
			If oSchedTask Is Nothing Then
				' No, so exit.
				Exit Sub
			Else
				' Yes, so lets update it.
				
				' UPDATE the Schedule Task with the values supplied
				With oSchedTask

					If Not Information.IsNothing(v_vCustomer) Then
						.Customer = v_vCustomer
					End If

					If Not Information.IsNothing(v_vDescription) Then
						.Description = v_vDescription
					End If

					If Not Information.IsNothing(v_vTaskDueDate) Then
						.TaskDueDate = v_vTaskDueDate
					End If

					If Not Information.IsNothing(v_vIsUrgent) Then
						.IsUrgent = v_vIsUrgent
					End If

					If Not Information.IsNothing(v_vTaskStatus) Then
						.TaskStatus = v_vTaskStatus
					End If

					If Not Information.IsNothing(v_vUserGroupID) Then
						.PmuserGroupID = CInt(v_vUserGroupID)
						' Get the User Group Caption
						sUserGroup = m_frmInterface.cboUserGroup.ItemUserGroupname(v_vUserGroupID)
						.UserGroup = sUserGroup
						v_vUserGroupID = sUserGroup
					End If

					If Not Information.IsNothing(v_vUserID) Then
						.UserID = v_vUserID
						Dim dbNumericTemp As Double
						If Double.TryParse(v_vUserID, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
							If StringsHelper.ToDoubleSafe(v_vUserID) < 1 Then
								sUser = ""
							Else
								sUser = m_frmInterface.cboUser.ItemUsername(v_vUserID)
							End If
						Else
							sUser = ""
						End If
						v_vUserID = sUser
					End If

					If Not Information.IsNothing(v_vNavProcessID) Then
						.PMNavProcessId = v_vNavProcessID
					End If

					If Not Information.IsNothing(v_vComponentObjectName) Then
						.ComponentObjectName = v_vComponentObjectName
					End If

					If Not Information.IsNothing(v_vComponentClassName) Then
						.ComponentClassName = v_vComponentClassName
					End If

					If Not Information.IsNothing(v_vDisplayIcon) Then
						.DisplayIcon = v_vDisplayIcon
					End If

					If Not Information.IsNothing(v_vIsViewOnlyTask) Then
						.IsViewOnlyTask = v_vIsViewOnlyTask
					End If

					If Not Information.IsNothing(v_vLinkedObjectName) Then
						.LinkedObjectName = v_vLinkedObjectName
					End If

					If Not Information.IsNothing(v_vLinkedClassName) Then
						.LinkedClassName = v_vLinkedClassName
					End If

					If Not Information.IsNothing(v_vLinkedCaption) Then
						.LinkedCaption = v_vLinkedCaption
					End If
					'DAK141299
					iIsVisible = .IsVisible
				End With
				
				' Release the reference
				oSchedTask = Nothing
				
				If iIsVisible = gPMConstants.PMEReturnCode.PMTrue Then
					' Get the Description for this Status
					sStatusDesc = TaskStatusDescription(v_vTaskStatus)
					
					' Update the Task Status in the List View
					m_frmInterface.UpdateScheduledTask(v_sKey:=sScheduledTaskKey, v_vTaskStatusDesc:=sStatusDesc, v_vTaskDueDate:=v_vTaskDueDate, v_vDescription:=v_vDescription, v_vCustomer:=v_vCustomer, v_vUserGroup:=v_vUserGroupID, v_vUser:=v_vUserID)
					
				End If
				
			End If
		
		
	End Sub
	
	' ***************************************************************** '
	' Name: DeleteScheduledTask
	'
	' Description: Deletes the details for a Scheduled Task in the
	'              List View and Collection.
	'
	' ***************************************************************** '
	'UPGRADE_NOTE: (7001) The following declaration (DeleteScheduledTask) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Sub DeleteScheduledTask(ByVal v_lPMWrkTaskInstanceCnt As Integer)
		'Dim sScheduledTaskKey As String = ""
		'DAK141299
		'Dim oSchedTask As ScheduledTask
		'Dim iIsVisible As gPMConstants.PMEReturnCode
		'
		'
		'Try 
			'
			' Generate the Collection Key for this Task Instance
			'sScheduledTaskKey = m_oSchedTasks.GenerateKey(v_lScheduledTaskCnt:=v_lPMWrkTaskInstanceCnt)
			'
			'DAK141299
			'oSchedTask = m_oSchedTasks.Item(sScheduledTaskKey)
			'iIsVisible = oSchedTask.IsVisible
			'
			' Delete the Scheduled Task from the Collection
			'm_oSchedTasks.Delete(sScheduledTaskKey)
			' Delete the Scheduled Task From the List
			'DAK141299
			'If iIsVisible = gPMConstants.PMEReturnCode.PMTrue Then
				'm_frmInterface.DeleteScheduledTask(v_sKey:=sScheduledTaskKey)
			'End If
		'
		'Catch excep As System.Exception
			'
			'
			'
			'
			' Log Error Message
			'DisplayErrorMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteScheduledTaskFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteScheduledTask", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			'
			'Exit Sub
			'
		'End Try
		'
	'End Sub
	
	
	' ***************************************************************** '
	' Name: SetTaskMenuOptions
	'
	' Description: Set the Task Menu options dependent on the currently
	'              selected Scheduled Task.
	' ***************************************************************** '
	Private Sub SetTaskMenuOptions()
		
		Dim bEditEnabled, bAssignEnabled, bViewEnabled As Boolean
		
		Dim oScheduledTask As ScheduledTask
		
		 
			
			bEditEnabled = False
			bAssignEnabled = False
			bViewEnabled = False
			
			oScheduledTask = m_oSchedTasks.Item(ScheduledTaskKey)
			CalcTaskMenuOptions(v_oScheduledTask:=oScheduledTask, r_bEditEnabled:=bEditEnabled, r_bAssignEnabled:=bAssignEnabled, r_bViewEnabled:=bViewEnabled)
			
			m_frmInterface.SetTaskMenuOptions(v_bEditEnabled:=bEditEnabled, v_bAssignEnabled:=bAssignEnabled, v_bViewEnabled:=bViewEnabled)
		
		
	End Sub
	
	' ***************************************************************** '
	' Name: CalcTaskMenuOptions
	'
	' Description: Set the Task Menu options dependent on the currently
	'              selected Scheduled Task.
	' ***************************************************************** '
	Private Sub CalcTaskMenuOptions(ByVal v_oScheduledTask As ScheduledTask, ByRef r_bEditEnabled As Boolean, ByRef r_bAssignEnabled As Boolean, ByRef r_bViewEnabled As Boolean)
		
		Dim lPMAuthorityLevel, lPMUserGroupID As Integer
		
		 
			
			' Get the User Group ID for this Task
			lPMUserGroupID = v_oScheduledTask.PmuserGroupID
			
			' If this User a System Administrator or Normal User
			m_lReturn = GetUserAuthority(r_lPMAuthorityLevel:=lPMAuthorityLevel, v_lUserGroupID:=lPMUserGroupID)
			
			' Set the Default Options dependent on the Task Status
			
			Select Case v_oScheduledTask.TaskStatus
				' New or Incomplete Tasks
				Case gPMConstants.PMEWrkManTaskStatus.pmeWMTSNew, gPMConstants.PMEWrkManTaskStatus.pmeWMTSIncomplete
					
					r_bEditEnabled = True
					r_bAssignEnabled = True
					r_bViewEnabled = True
					
					' In Progress
				Case gPMConstants.PMEWrkManTaskStatus.pmeWMTSInProgress
					r_bViewEnabled = True
					
					' Complete
				Case gPMConstants.PMEWrkManTaskStatus.pmeWMTSComplete
					r_bViewEnabled = True
					
			End Select
		
		
	End Sub
	
	' ***************************************************************** '
	' Name: TaskStatusDescription
	'
	' Description: Returns the Task Status Description for the Supplied
	'              Task Status.
	'
	' ***************************************************************** '
	Private Function TaskStatusDescription(ByVal eTaskStatus As gPMConstants.PMEWrkManTaskStatus) As String
		
		Dim result As String = String.Empty
		 
			
			result = "Unknown"
			
			Select Case eTaskStatus
				Case gPMConstants.PMEWrkManTaskStatus.pmeWMTSNew
					result = ACTaskStatusDescNew
				Case gPMConstants.PMEWrkManTaskStatus.pmeWMTSInProgress
					result = ACTaskStatusDescInProgress
				Case gPMConstants.PMEWrkManTaskStatus.pmeWMTSIncomplete
					result = ACTaskStatusDescInComplete
				Case gPMConstants.PMEWrkManTaskStatus.pmeWMTSComplete
					result = ACTaskStatusDescComplete
			End Select
			
			Return result
		
		
	End Function
	
	' ***************************************************************** '
	' Name: EditViewAssignSchedTask
	'
	' Description: Displays the Task Instance form for a Scheduled Task
	'
	' ***************************************************************** '
	Private Function EditViewAssignSchedTask(ByVal v_lAction As Integer, ByVal v_lPMWrkTaskInstanceCnt As Integer) As Integer
		
		Dim result As Integer = 0
		Dim sCurrentlyLockedBy As String = ""

		
		 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			If v_lAction = gPMConstants.PMEComponentAction.PMAdd Then
				Return result
			End If
			
			' Lock the Task Instance

			m_lReturn = m_oBusiness.LockTaskInstance(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt, r_sCurrentlyLockedBy:=sCurrentlyLockedBy)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				DisplayErrorMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Scheduled Task is Locked by user " & sCurrentlyLockedBy.Trim())
				Return result
			End If
			
			' Display the Form
			m_lReturn = DisplayTaskInstanceForm(v_lAction:=v_lAction, r_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				DisplayErrorMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to display the Task Instance Form.")
				Return result
			End If
			
			' Lock the Task Instance

			m_lReturn = m_oBusiness.UnLockTaskInstance(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				DisplayErrorMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to UnLock Scheduled Task. Contact your system Administrator.")
				Return result
			End If
			
			Return result
		
	End Function
	
	' ***************************************************************** '
	' Name: DisplayTaskInstanceForm
	'
	' Description: Displays the Task Instance Form in the Mode required.
	' ***************************************************************** '
	Private Function DisplayTaskInstanceForm(ByVal v_lAction As Integer, ByRef r_lPMWrkTaskInstanceCnt As Integer, Optional ByVal v_lPMWrkTaskGroupID As Integer = 0, Optional ByVal v_lPMWrkTaskID As Integer = 0) As Integer
		Dim result As Integer = 0

		Dim oTaskInstance As iPMWrkTaskInstance.Interface_Renamed
		'Dim oTaskInstance As iPMWrkTaskInstance.Interface
		
		 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Create the Component
			Dim temp_oTaskInstance As Object
			m_lReturn = m_oObjectManager.GetInstance(temp_oTaskInstance, sClassName:="iPMWrkTaskInstance.Interface_Renamed", vinstancemanager:=gPMConstants.PMGetLocalInterface)
			oTaskInstance = temp_oTaskInstance
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Set Process Modes

			m_lReturn = oTaskInstance.SetProcessModes(vTask:=v_lAction, vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired, vEffectiveDate:=DateTime.Now)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' If Add
			If v_lAction = gPMConstants.PMEComponentAction.PMAdd Then
				' Do we Know what Task Group/Task to create
				If (v_lPMWrkTaskGroupID > 0) And (v_lPMWrkTaskID > 0) Then
					' Yes, so set the Properties.

					oTaskInstance.PMWrkTaskGroupId = v_lPMWrkTaskGroupID

					oTaskInstance.PMWrkTaskId = v_lPMWrkTaskID
				End If
			Else
				' Edit, View, ReAssign Mode so set the Key

				oTaskInstance.PMWrkTaskInstanceCnt = r_lPMWrkTaskInstanceCnt
			End If
			
			' Start the Form

			m_lReturn = oTaskInstance.Start
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				DisplayErrorMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Start Task Instance Form:- iPMWrkTaskInstance.Interface")

            oTaskInstance.Dispose()
				oTaskInstance = Nothing
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' If the User Canceled then exit as we do not need
			' to Refresh the Form details.

			If oTaskInstance.Status = gPMConstants.PMEReturnCode.PMCancel Then
				r_lPMWrkTaskInstanceCnt = 0

            oTaskInstance.Dispose()
				oTaskInstance = Nothing
				Return result
			End If
			
			' Refreh the Form/Collection if we need to.
			Select Case v_lAction
				' View
				Case gPMConstants.PMEComponentAction.PMView
					' No Refresh Required.
					
					' Add
				Case gPMConstants.PMEComponentAction.PMAdd
					With oTaskInstance

						r_lPMWrkTaskInstanceCnt = .PMWrkTaskInstanceCnt


















						m_lReturn = AddScheduledTask(v_lPMWrkTaskInstanceCnt:=.PMWrkTaskInstanceCnt, v_sCustomer:=.Customer, v_sDescription:=.Description, v_dtTaskDueDate:=.duedate, v_iIsUrgent:=.IsUrgent, v_iTypeOfTask:=.TypeOfTask, v_iIsSystem:=.IsSystemTask, v_iTaskStatus:=.TaskStatus, v_lUserGroupID:=.PmuserGroupID, v_vUserID:=.UserID, v_lNavProcessID:=.PMNavProcessId, v_sComponentObjectName:=.ComponentObjectName, v_sComponentClassName:=.ComponentClassName, v_lDisplayIcon:=.DisplayIcon, v_iIsViewOnlyTask:=.IsViewOnlyTask, v_sLinkedObjectName:=.LinkedObjectName, v_sLinkedClassName:=.LinkedClassName, v_sLinkedCaption:=.LinkedCaption, v_iIsVisible:=gPMConstants.PMEReturnCode.PMTrue, v_bDisplayOnForm:=True)
					End With
					
					' Anything Else i.e. Edit, Assign
				Case Else
					
					With oTaskInstance
















						UpdateScheduledTask(v_lPMWrkTaskInstanceCnt:=.PMWrkTaskInstanceCnt, v_vCustomer:=.Customer, v_vDescription:=.Description, v_vTaskDueDate:=.duedate, v_vIsUrgent:=.IsUrgent, v_vTaskStatus:=.TaskStatus, v_vUserGroupID:=.PmuserGroupID, v_vUserID:=.UserID, v_vNavProcessID:=.PMNavProcessId, v_vComponentObjectName:=.ComponentObjectName, v_vComponentClassName:=.ComponentClassName, v_vDisplayIcon:=.DisplayIcon, v_vIsViewOnlyTask:=.IsViewOnlyTask, v_vLinkedObjectName:=.LinkedObjectName, v_vLinkedClassName:=.LinkedClassName, v_vLinkedCaption:=.LinkedCaption)
					End With
					
			End Select
			

        oTaskInstance.Dispose()
			oTaskInstance = Nothing
			
			Return result
		
	End Function
	
	' ***************************************************************** '
	' Name: AddViewSchedTaskLog
	'
	' Description: Displays the Task Instance Log form for a Scheduled Task
	'
	' ***************************************************************** '
	'UPGRADE_NOTE: (7001) The following declaration (AddViewSchedTaskLog) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Function AddViewSchedTaskLog(ByVal v_lAction As Integer, ByVal v_lPMWrkTaskInstanceCnt As Integer) As Integer
		'Dim result As Integer = 0
		'Dim iPMWrkTaskInstLog As Object
		'

		'Dim oTaskLog As iPMWrkTaskInstLog.Interface_Renamed
		'
		'Try 
			'
			'result = gPMConstants.PMEReturnCode.PMTrue
			'
			' Create the Component
			'Dim temp_oTaskLog As Object
			'm_lReturn = m_oObjectManager.GetInstance(temp_oTaskLog, sClassName:="iPMWrkTaskInstLog.Interface_Renamed", vinstancemanager:=gPMConstants.PMGetLocalInterface)
			'oTaskLog = temp_oTaskLog
			'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				'Return gPMConstants.PMEReturnCode.PMFalse
			'End If
			'
			' Set Process Modes

			'm_lReturn = oTaskLog.SetProcessModes(vTask:=v_lAction, vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired, vEffectiveDate:=DateTime.Now)
			'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				'Return gPMConstants.PMEReturnCode.PMFalse
			'End If
			'
			' Set the key.

			'oTaskLog.PMWrkTaskInstanceCnt = v_lPMWrkTaskInstanceCnt
			'
			' Start the Form

			'm_lReturn = oTaskLog.Start
			'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				'DisplayErrorMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Start Task Log Form:- iPMWrkTaskInstLog.Interface")

				'm_lReturn = oTaskLog.Terminate()
				'oTaskLog = Nothing
				'Return gPMConstants.PMEReturnCode.PMFalse
			'End If
			'

			'm_lReturn = oTaskLog.Terminate()
			'oTaskLog = Nothing
			'
			'Return result
		'
		'Catch excep As System.Exception
			'
			'
			'
			'result = gPMConstants.PMEReturnCode.PMError
			'
			' Log Error Message
			'DisplayErrorMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddViewSchedTaskLogFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddViewSchedTaskLog", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			'
			'Return result
			'
		'End Try
	'End Function
	
	' ***************************************************************** '
	' Name: DisplayErrorMessage
	'
	' Description: Displays an error message on the Status Bar.
	' ***************************************************************** '
Private Sub DisplayErrorMessage(ByRef iType As Integer, ByRef sMsg As String, Optional ByRef vApp As Object = Nothing, Optional ByRef vClass As Object = Nothing, Optional ByRef vMethod As Object = Nothing, Optional ByRef vErrNo As Object = Nothing, Optional ByRef vErrDesc As Object = Nothing) 
		
		 
			
			m_frmInterface.UpdateStatusBar(v_vErrorMsg:=sMsg)
			
			If iType <> gPMConstants.PMELogLevel.PMLogInfo Then



            gPMFunctions.LogMessageToFile(sUserName:=Username, iType:=iType, sMsg:=sMsg, vApp:=CStr(vApp), vClass:=CStr(vClass), vMethod:=CStr(vMethod))
			End If
		
		
	End Sub
	
	' ***************************************************************** '
	' Name: GetRegistrySettings
	'
	' Description:
	' ***************************************************************** '
	Private Sub GetRegistrySettings()
		Dim sViewToolbar, sViewStatusBar, sViewGridLines As String
		'DAK110100
		Dim sIsAutoRefresh, sRefreshRate As String
		
		 
			
			' Get the View Toolbar setting
			m_lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:=gPMConstants.ACWrkManRegViewToolbar, r_sSettingValue:=sViewToolbar, v_sSubKey:=gPMConstants.ACWrkManRegSubKey)
			
			' Is there a setting
			Dim dbNumericTemp As Double
			If sViewToolbar.Trim() = "" Then
				' No, so display it
				m_frmInterface.ViewToolbar = True
				' Is it Numeric
			ElseIf (Double.TryParse(sViewToolbar, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)) Then 
				' Is it True or False
				If CInt(sViewToolbar) = gPMConstants.PMEReturnCode.PMTrue Then
					' True, so display
					m_frmInterface.ViewToolbar = True
				Else
					' False, so do NOT display
					m_frmInterface.ViewToolbar = False
				End If
			Else
				' Not Numeric so display it
				m_frmInterface.ViewToolbar = True
			End If
			
			' Get the View Status Bar setting
			m_lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:=gPMConstants.ACWrkManRegViewStatusBar, r_sSettingValue:=sViewStatusBar, v_sSubKey:=gPMConstants.ACWrkManRegSubKey)
			
			' Is there a setting
			Dim dbNumericTemp2 As Double
			If sViewStatusBar.Trim() = "" Then
				' No, so display it
				m_frmInterface.ViewStatusBar = True
				' Is it Numeric
			ElseIf (Double.TryParse(sViewStatusBar, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2)) Then 
				' Is it True or False
				If CInt(sViewStatusBar) = gPMConstants.PMEReturnCode.PMTrue Then
					' True, so display
					m_frmInterface.ViewStatusBar = True
				Else
					' False, so do NOT display
					m_frmInterface.ViewStatusBar = False
				End If
			Else
				' Not Numeric so display it
				m_frmInterface.ViewStatusBar = True
			End If
			
			' Get the View Grid Lines setting
			m_lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:=gPMConstants.ACWrkManRegViewGridLines, r_sSettingValue:=sViewGridLines, v_sSubKey:=gPMConstants.ACWrkManRegSubKey)
			
			' Is there a setting
			Dim dbNumericTemp3 As Double
			If sViewGridLines.Trim() = "" Then
				' No, so display it
				m_frmInterface.ViewGridLines = True
				' Is it Numeric
			ElseIf (Double.TryParse(sViewGridLines, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3)) Then 
				' Is it True or False
				If CInt(sViewGridLines) = gPMConstants.PMEReturnCode.PMTrue Then
					' True, so display
					m_frmInterface.ViewGridLines = True
				Else
					' False, so do NOT display
					m_frmInterface.ViewGridLines = False
				End If
			Else
				' Not Numeric so display it
				m_frmInterface.ViewGridLines = True
			End If
			
			' Get the Auto Refresh setting
			m_lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:=gPMConstants.ACWrkManRegIsAutoRefresh, r_sSettingValue:=sIsAutoRefresh, v_sSubKey:=gPMConstants.ACWrkManRegSubKey)
			
			' Is there a setting
			Dim dbNumericTemp4 As Double
			If sIsAutoRefresh.Trim() = "" Then
				' No, so display it
				m_frmInterface.IsAutoRefresh = True
				' Is it Numeric
			ElseIf (Double.TryParse(sIsAutoRefresh, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp4)) Then 
				' Is it True or False
				If CInt(sIsAutoRefresh) = gPMConstants.PMEReturnCode.PMTrue Then
					' True, so display
					m_frmInterface.IsAutoRefresh = True
				Else
					' False, so do NOT display
					m_frmInterface.IsAutoRefresh = False
				End If
			Else
				' Not Numeric so display it
				m_frmInterface.IsAutoRefresh = True
			End If
			
			' Get the Refresh Rate setting
			m_lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:=gPMConstants.ACWrkManRegRefreshRate, r_sSettingValue:=sRefreshRate, v_sSubKey:=gPMConstants.ACWrkManRegSubKey)
			
			' Is there a setting
			Dim dbNumericTemp5 As Double
			If sRefreshRate.Trim() = "" Then
				' No, so default it
				m_frmInterface.RefreshRate = 1
				' Is it Numeric
			ElseIf (Double.TryParse(sRefreshRate, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp5)) Then 
				'It should be between 1 and 60
				If CInt(sRefreshRate) < 1 Then
					m_frmInterface.RefreshRate = 1
				ElseIf CInt(sRefreshRate) > 60 Then 
					m_frmInterface.RefreshRate = 60
				Else
					m_frmInterface.RefreshRate = CInt(sRefreshRate)
				End If
			Else
				' Not Numeric so default it
				m_frmInterface.RefreshRate = 1
			End If
		
		
	End Sub
	
	' ***************************************************************** '
	' Name: SetRegistrySettings
	'
	' Description:
	' ***************************************************************** '
	'UPGRADE_NOTE: (7001) The following declaration (SetRegistrySettings) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Sub SetRegistrySettings()
		'Dim sViewSplash, sViewQuickStart, sViewAvailableTasks As String
		'DAK241299
		'Dim sViewToolbar, sViewStatusBar, sViewGridLines As String
		'Dim sViewGraphics As String
		'DAK110100
		'Dim sIsAutoRefresh, sRefreshRate As String
		'
		'
		'Try 
			'
			' Set the View Toolbar Setting
			'If m_frmInterface.ViewToolbar Then
				'sViewToolbar = CStr(gPMConstants.PMEReturnCode.PMTrue)
			'Else
				'sViewToolbar = CStr(gPMConstants.PMEReturnCode.PMFalse)
			'End If
			'
			'm_lReturn = gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:=gPMConstants.ACWrkManRegViewToolbar, v_sSettingValue:=sViewToolbar, v_sSubKey:=gPMConstants.ACWrkManRegSubKey)
			'
			' Set the View Status Bar Setting
			'If m_frmInterface.ViewStatusBar Then
				'sViewStatusBar = CStr(gPMConstants.PMEReturnCode.PMTrue)
			'Else
				'sViewStatusBar = CStr(gPMConstants.PMEReturnCode.PMFalse)
			'End If
			'
			'm_lReturn = gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:=gPMConstants.ACWrkManRegViewStatusBar, v_sSettingValue:=sViewStatusBar, v_sSubKey:=gPMConstants.ACWrkManRegSubKey)
			'
			' Set the View Grid Lines Setting
			'If m_frmInterface.ViewGridLines Then
				'sViewGridLines = CStr(gPMConstants.PMEReturnCode.PMTrue)
			'Else
				'sViewGridLines = CStr(gPMConstants.PMEReturnCode.PMFalse)
			'End If
			'
			'm_lReturn = gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:=gPMConstants.ACWrkManRegViewGridLines, v_sSettingValue:=sViewGridLines, v_sSubKey:=gPMConstants.ACWrkManRegSubKey)
			'
			' Set the View Graphics Setting
			'    If (m_frmInterface.ViewGraphics = True) Then
			'        sViewGraphics = PMTrue
			'    Else
			'        sViewGraphics = PMFalse
			'    End If
			'
			'    m_lReturn& = SetPMRegSetting( _
			'v_lPMERegSettingRoot:=pmeRSRCurrentUser, _
			'v_lPMEProductFamily:=pmePFSiriusArchitecture, _
			'v_lPMERegSettingLevel:=pmeRSLClient, _
			'v_sSettingName:=ACRegKeyViewGraphics, _
			'v_sSettingValue:=sViewGraphics)
			'
			' Set the Is Auto Refresh Setting
			'If m_frmInterface.IsAutoRefresh Then
				'sIsAutoRefresh = CStr(gPMConstants.PMEReturnCode.PMTrue)
			'Else
				'sIsAutoRefresh = CStr(gPMConstants.PMEReturnCode.PMFalse)
			'End If
			'
			'm_lReturn = gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:=gPMConstants.ACWrkManRegIsAutoRefresh, v_sSettingValue:=sIsAutoRefresh, v_sSubKey:=gPMConstants.ACWrkManRegSubKey)
			'
			' Set the Refresh Rate Setting
			'sRefreshRate = CStr(m_frmInterface.RefreshRate)
			'm_lReturn = gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:=gPMConstants.ACWrkManRegRefreshRate, v_sSettingValue:=sRefreshRate, v_sSubKey:=gPMConstants.ACWrkManRegSubKey)
		'
		'Catch excep As System.Exception
			'
			'
			'
			'
			' Log Error Message
			'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetRegistrySettingsFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetRegistrySettings", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			'
			'Exit Sub
			'
		'End Try
		'
	'End Sub
	
	' PRIVATE Methods (End)
	
	Public Sub New()
		MyBase.New()
		
		'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
		'Try 
			'
			' Class Initialise
		'
		'Catch excep As System.Exception
			'
			'
			'
			' Error.
			'
			' Log Error Message
			'DisplayErrorMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			'
			'Exit Sub
			'
		'End Try
		
	End Sub
	
	Protected Overrides Sub Finalize()
		Dispose(False)
	End Sub

	
	Private Sub m_frmInterface_FormClose() Handles m_frmInterface.FormClose
		
        Dispose()
		
	End Sub
	
	Private Sub m_frmInterface_RefreshScheduledTasks(ByVal v_bForceRefresh As Boolean) Handles m_frmInterface.RefreshScheduledTasks
		
		' If the Form is not shown yet, then do not Refresh the Scheduled Tasks.
		If m_frmInterface.FormDisplayed Then
			' Populate the Scheduled Tasks
			m_lReturn = PopulateSchedTasks(v_bForceRefresh:=v_bForceRefresh)
		End If
		
	End Sub
	
	Private Sub m_frmInterface_ScheduledTaskAction(ByVal eAction As MainModule.ACESchedTaskAction) Handles m_frmInterface.ScheduledTaskAction
		
		
		' Is there a Selected Scheduled Task
		If ScheduledTaskKey = "" Then
			' No, so do Nothing
			Exit Sub
		End If
		
		' Get a Reference to the loaded Scheduled Task
		Dim oSchedTask As ScheduledTask = m_oSchedTasks.Item(ScheduledTaskKey)
		
		' If we haven't got it for some reason, then exit.
		If oSchedTask Is Nothing Then
			Exit Sub
		End If
		
		' If the User has Double Clicked on a Task which is already in Progress
		' then ingore.
		If (oSchedTask.TaskStatus = gPMConstants.PMEWrkManTaskStatus.pmeWMTSInProgress) And (eAction = MainModule.ACESchedTaskAction.aceSTAStart) Then
			oSchedTask = Nothing
			Exit Sub
		End If
		
		' If the User has Double Clicked on a Complete Task
		' then Delete It.
		If (oSchedTask.TaskStatus = gPMConstants.PMEWrkManTaskStatus.pmeWMTSComplete) And (eAction = MainModule.ACESchedTaskAction.aceSTAStart) Then
			eAction = MainModule.ACESchedTaskAction.aceSTADelete
		End If
		
		' If the User has Double Clicked on a Memo Task
		If (oSchedTask.TypeOfTask = gPMConstants.PMEWrkManTaskType.pmeWMTTMemo) And (eAction = MainModule.ACESchedTaskAction.aceSTAStart) Then
			
			' If the Memo Task has NO Assigned User
			If oSchedTask.UserID = "" Then
				' Assign It
				eAction = MainModule.ACESchedTaskAction.aceSTAAssign
				
				' Else If the Task is Assigned to ME
			ElseIf (CDbl(oSchedTask.UserID) = UserID) Then 
				' Start it
				
				' Otherwise, it is NOT Assigned to ME
			Else
				' Assign It
				eAction = MainModule.ACESchedTaskAction.aceSTAAssign
			End If
			
		End If
		
		' Get the Task Instance Cnt.
		Dim lPMWrkTaskInstanceCnt As Integer = oSchedTask.PMWrkTaskInstanceCnt
		' Release Reference.
		oSchedTask = Nothing
		
		
		Select Case eAction
			Case MainModule.ACESchedTaskAction.aceSTAAssign
				' For the Puroposes of this Form Delete = ReAssign
				m_lReturn = EditViewAssignSchedTask(v_lAction:=gPMConstants.PMEComponentAction.PMDelete, v_lPMWrkTaskInstanceCnt:=lPMWrkTaskInstanceCnt)
				
			Case MainModule.ACESchedTaskAction.aceSTAEdit
				m_lReturn = EditViewAssignSchedTask(v_lAction:=gPMConstants.PMEComponentAction.PMEdit, v_lPMWrkTaskInstanceCnt:=lPMWrkTaskInstanceCnt)
				
			Case MainModule.ACESchedTaskAction.aceSTAView
				m_lReturn = EditViewAssignSchedTask(v_lAction:=gPMConstants.PMEComponentAction.PMView, v_lPMWrkTaskInstanceCnt:=lPMWrkTaskInstanceCnt)
				
			Case Else
				Exit Sub
				
		End Select
		
	End Sub
	
	Private Sub m_frmInterface_ScheduledTaskClick(ByVal v_sScheduledTaskKey As String) Handles m_frmInterface.ScheduledTaskClick
		
		' Store the Scheduled Task Key that was clicked on
		ScheduledTaskKey = v_sScheduledTaskKey
		
		' Set the Task Menu options accordingly
		SetTaskMenuOptions()
		
	End Sub
	
	Private Sub m_frmInterface_ScheduledTaskRightClick(ByVal v_sScheduledTaskKey As String) Handles m_frmInterface.ScheduledTaskRightClick
		
		' Store the Scheduled Task Key that was clicked on
		ScheduledTaskKey = v_sScheduledTaskKey
		
		' Set the Task Menu options accordingly
		SetTaskMenuOptions()
		
		' Display the Task Menu
		m_frmInterface.DisplayTaskMenu()
		
	End Sub
	
	Public Function GetSummary(ByRef vSummaryArray As Object) As Integer
		
		Return gPMConstants.PMEReturnCode.PMTrue
		
	End Function
End Class

