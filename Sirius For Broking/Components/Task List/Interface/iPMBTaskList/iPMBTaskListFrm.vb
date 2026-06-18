Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
Partial Public Class frmInterface
    Inherits System.Windows.Forms.Form
    '
    ' History:
    ' CJB 050405 PN14908 StartScheduledTask changed to cater for no ProcessID value when there is a
    '            XMLfile value. Prevent type mismatch errors now.
    '

    Const ACClass As String = "frmInterface"

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_lErrorNumber As gPMConstants.PMEReturnCode

    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lNavigate As gPMConstants.PMENavigateButtonStatus
    Private m_lProcessMode As gPMConstants.PMEProcessMode
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' Status members
    Private m_sProcessStatus As New FixedLengthString(2)
    Private m_sMapStatus As New FixedLengthString(2)
    Private m_sStepStatus As New FixedLengthString(2)

    'Standard Return Code
    Private m_lReturn As Integer

    Private m_lPartyCnt As Integer
    Private m_lInsuranceFileCnt As Integer
    Private m_vArray(,) As Object
    Private m_lTaskID As Integer
    Private m_lAuthority As Integer
    Private m_lFSAComplaintFolderCnt As Integer

    'Business Component


    'Modified by Sumeet Singh on 5/19/2010 7:23:39 PM to do list (Iteration 3)
    'Private m_oBusiness As bSIRTaskList.Business
    Private m_oBusiness As Object

    Private m_oNavigator As Navigator

    'Private WithEvents m_oNavigator As iPMNavigator.NavigateControl
    'Private WithEvents m_oComponent As iPMWrkComponentStarter.StartControl

    Public Property InsuranceFileCnt() As Integer
        Get

            Return m_lInsuranceFileCnt

        End Get
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the calling application name.
            m_lInsuranceFileCnt = Value

        End Set
    End Property
    Public Property FSAComplaintFolderCnt() As Integer
        Get

            Return m_lFSAComplaintFolderCnt

        End Get
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the calling application name.
            m_lFSAComplaintFolderCnt = Value

        End Set
    End Property

    ' PUBLIC Property Procedures (Begin)

    Public ReadOnly Property ErrorNumber() As Integer
        Get

            ' Standard Property.

            ' Return any error number that might have
            ' occurred on the interface.
            Return m_lErrorNumber

        End Get
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Standard Property.

            ' Set the calling application name.
            m_sCallingAppName = Value

        End Set
    End Property
    'Modified by Sumeet Singh on 5/19/2010 6:01:57 PM to do list (Iteration 3)
    'Public WriteOnly Property Business() As bSIRTaskList.Business
    Public WriteOnly Property Business() As Object
        'Modified by Sumeet Singh on 5/19/2010 6:02:22 PM to do list (Iteration 3)
        'Set(ByVal Value As bSIRTaskList.Business)
        Set(ByVal Value As Object)

            m_oBusiness = Value

        End Set
    End Property

    Public ReadOnly Property Status() As Integer
        Get

            ' Standard Property.

            ' Return the interface exit status.
            Return m_lStatus

        End Get
    End Property

    Public Property Task() As Integer
        Get

            ' Return the objects task.
            Return m_iTask

        End Get
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the objects task.
            m_iTask = Value

        End Set
    End Property

    Public WriteOnly Property Navigate() As Integer
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the navigate flag.
            m_lNavigate = Value

        End Set
    End Property

    Public WriteOnly Property ProcessMode() As Integer
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the process mode.
            m_lProcessMode = Value

        End Set
    End Property

    Public WriteOnly Property TransactionType() As String
        Set(ByVal Value As String)

            ' Standard Property.

            ' Set the type of business.
            m_sTransactionType = Value

        End Set
    End Property

    Public WriteOnly Property EffectiveDate() As Date
        Set(ByVal Value As Date)

            ' Standard Property.

            ' Set the effective date.
            m_dtEffectiveDate = Value

        End Set
    End Property

    Public ReadOnly Property StepStatus() As String
        Get

            ' Standard Property.

            ' Return the Steps Status
            Return m_sStepStatus.Value

        End Get
    End Property
    ' {* USER DEFINED CODE (Begin) *}
    Public WriteOnly Property PartyCnt() As Integer
        Set(ByVal Value As Integer)

            ' Set the objects parameter value.
            m_lPartyCnt = Value

        End Set
    End Property

    ' {* USER DEFINED CODE (END) *}

    ' ***************************************************************** '
    ' Name: SetStatus (Standard Method)
    '
    ' Description: Set the Process, Map and Step status.
    ' Note:        A Property Get is provided for the Step Status only
    '              as this is the only one which this component can
    '              alter directly.
    ' ***************************************************************** '
    Public Function SetStatus(ByRef sProcessStatus As String, ByRef sMapStatus As String, ByRef sStepStatus As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the current Status settings.
            m_sProcessStatus.Value = sProcessStatus.Trim()
            m_sMapStatus.Value = sMapStatus.Trim()
            m_sStepStatus.Value = sStepStatus.Trim()

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetStatus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' {* USER DEFINED CODE (Begin) *}
    ' ***************************************************************** '
    ' Name: GetInterfaceDetails
    '
    ' Description: Gets the interface details and sets the appropriate
    '              sytle.
    '
    ' ***************************************************************** '
    Private Function GetInterfaceDetails() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            ' Get the interface details from the
            ' business object.
            m_lReturn = GetTaskList()
            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the details.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Assign the details from the business object
            ' to the interface.
            m_lReturn = DataToInterface()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the details.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the interface details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInterfaceDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: GetTaskList
    '
    ' Description: Retrieves the details from the business object.
    '
    ' ***************************************************************** '
    Private Function GetTaskList(Optional ByVal bSetup As Boolean = True) As Integer
        Dim result As Integer = 0
        Dim vTasks As Object
        Dim sType As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            ' Get the details from the business object.

            ' {* USER DEFINED CODE (Begin) *}

            ' Turn on Row Select and Grid Lines
            m_lReturn = SetExtraListViewProperties(v_hWndList:=lstScheduledTasks.Handle.ToInt32(), v_vShowRowSelect:=True, v_vShowGridLines:=True)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTaskList")

                Return result
            End If

            ' Get list of tasks dependant on Party or Policy or Complaint

            m_lReturn = m_oBusiness.GetAvailableTasks(r_vArray:=vTasks, m_lPartyCnt:=m_lPartyCnt, m_lInsuranceFileCnt:=InsuranceFileCnt, m_lFSAComplaintFolderCnt:=FSAComplaintFolderCnt)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTaskList")

                Return result
            End If

            ' Assign the tasks to the modular array

            m_vArray = vTasks

            ' Setup the form
            If bSetup Then
                m_lReturn = SetFormForDisplay()
            End If

            ' Populate the list view
            PopulateListView()

            ' Sort by hidden date column
            m_lReturn = ListViewSortByDate(v_oListView:=lstScheduledTasks, v_iSourceColumn:=8, v_iDirection:=SortOrder.Ascending)

            ' {* USER DEFINED CODE (End) *}

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTaskList")

                Return result
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTaskList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: DataToInterface
    '
    ' Description: Takes Data from Business & Set Keys and populates Form
    '
    ' ***************************************************************** '
    Private Function DataToInterface() As Integer

        Return gPMConstants.PMEReturnCode.PMTrue

    End Function

    ' ***************************************************************** '
    ' Name: SetFormForDisplay
    '
    ' Description: Gets the Form ready for display.
    '
    ' ***************************************************************** '
    Private Function SetFormForDisplay() As Integer
        Dim result As Integer = 0
        Dim lReturn, lPMAuthorityLevel, lPMUserGroupID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oNavigator = New Navigator()
            'Set m_oNavigator = New iPMNavigator.NavigateControl
            'Set m_oComponent = New iPMWrkComponentStarter.StartControl

            With Me
                ' Populate the Task Status
                .cboTaskStatus.Items.Add(ACListTaskTypeAllButComplete)
                .cboTaskStatus.Items.Add(ACListTaskTypeAll)
                .cboTaskStatus.Items.Add(ACListTaskTypeNew)
                .cboTaskStatus.Items.Add(ACListTaskTypeInProgress)
                .cboTaskStatus.Items.Add(ACListTaskTypeInComplete)
                .cboTaskStatus.Items.Add(ACListTaskTypeComplete)
                .cboTaskStatus.SelectedIndex = 0

                'Developer Guie No 151
                Dim listindex As Integer
                ' Populate the Date Range
                listindex = .cboDateRange.Items.Add(New VB6.ListBoxItem(ACDateRangeDescAll, ACDateRangeIndexAll))

                listindex = .cboDateRange.Items.Add(New VB6.ListBoxItem(ACDateRangeDescToday, ACDateRangeIndexToday))
                listindex = .cboDateRange.Items.Add(New VB6.ListBoxItem(ACDateRangeDescNext1, ACDateRangeIndexNext1))
                listindex = .cboDateRange.Items.Add(New VB6.ListBoxItem(ACDateRangeDescNext2, ACDateRangeIndexNext2))
                listindex = .cboDateRange.Items.Add(New VB6.ListBoxItem(ACDateRangeDescNext3, ACDateRangeIndexNext3))
                listindex = .cboDateRange.Items.Add(New VB6.ListBoxItem(ACDateRangeDescNext4, ACDateRangeIndexNext4))
                listindex = .cboDateRange.Items.Add(New VB6.ListBoxItem(ACDateRangeDescNext5, ACDateRangeIndexNext5))
                listindex = .cboDateRange.Items.Add(New VB6.ListBoxItem(ACDateRangeDescNext6, ACDateRangeIndexNext6))
                listindex = .cboDateRange.Items.Add(New VB6.ListBoxItem(ACDateRangeDescNext7, ACDateRangeIndexNext7))
                listindex = .cboDateRange.Items.Add(New VB6.ListBoxItem(ACDateRangeDescNext14, ACDateRangeIndexNext14))
                listindex = .cboDateRange.Items.Add(New VB6.ListBoxItem(ACDateRangeDescNext28, ACDateRangeIndexNext28))

                ' Default is All Dates
                .cboDateRange.SelectedIndex = ACDateRangeIndexAll

                ' Get the User Authority
                lReturn = GetUserAuthority(r_lPMAuthorityLevel:=lPMAuthorityLevel)

                m_lAuthority = lPMAuthorityLevel

                ' Add an All Groups Entry in the UserGroups List
                ' and Refresh the List of Users
                .cboUserGroup.FirstItem = ACUserGroupAllGroups
                .cboUserGroup.RefreshList()

                ' Get the Currently Selected User GroupID
                lPMUserGroupID = .cboUserGroup.UserGroupID

                .cboUser.SingleUserID = 0
                .cboUser.Enabled = True
                If lPMUserGroupID > 0 Then
                    .cboUser.FirstItem = "All Group Users"
                    .cboUser.PMUserGroupID = lPMUserGroupID
                Else
                    .cboUser.FirstItem = "All Users"
                    .cboUser.PMUserGroupID = 0
                End If


                ' Refresh the List.
                .cboUser.RefreshList()
                '.cboUser.UserID = g_oObjectManager.UserID

            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetFormForDisplayFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFormForDisplay", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' PRIVATE Methods (Begin)

    Private Sub cboDateRange_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboDateRange.SelectedIndexChanged

        PopulateListView()

    End Sub

    Private Sub cboTaskStatus_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboTaskStatus.SelectedIndexChanged

        PopulateListView()

    End Sub

    Private Sub cboUser_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboUser.Click

        PopulateListView()

    End Sub

    Private Sub cboUserGroup_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboUserGroup.Click


        cboUser.SingleUserID = 0
        cboUser.Enabled = True
        If cboUserGroup.UserGroupID > 0 Then
            cboUser.FirstItem = "All Group Users"
            cboUser.PMUserGroupID = cboUserGroup.UserGroupID
        Else
            cboUser.FirstItem = "All Users"
            cboUser.PMUserGroupID = 0
        End If

        PopulateListView()

    End Sub

    Private Sub frmInterface_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm = eventSender

            uctPMResizer1.SetControlResizeOption("lstScheduledTasks", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROSizeOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
            uctPMResizer1.SetControlResizeOption("cboDateRange", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCRONoResize, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
            uctPMResizer1.SetControlResizeOption("cboTaskStatus", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCRONoResize, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
            uctPMResizer1.SetControlResizeOption("cboUser", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCRONoResize, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
            uctPMResizer1.SetControlResizeOption("cboUserGroup", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCRONoResize, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)

        End If
    End Sub

    Private Sub Form_Initialize_Renamed()
        m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue
    End Sub


    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
        ' Forms load event.

        Try

            ' Check if we have had an error so far.
            ' Possibly creating the business object.
            If m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
                ' We have already encountered an error,
                ' so we MUST exit now.
                Exit Sub
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Set the process modes for the busines object.

            m_lReturn = m_oBusiness.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the process modes for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")

                Exit Sub
            End If


            ' Set the business keys.
            ' {* USER DEFINED CODE (Begin) *}
            ' {* USER DEFINED CODE (End) *}

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' Gets the interface details to be displayed.
            m_lReturn = GetInterfaceDetails()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the interface details.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception



            ' Error Section

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub
        End Try

    End Sub


    ' ***************************************************************** '
    ' Name: GetUserAuthority
    '
    ' Description: Returns whether the User is a Sys Admin or Supervisor
    '              or Normal User.
    ' ***************************************************************** '
    Public Function GetUserAuthority(ByRef r_lPMAuthorityLevel As Integer, Optional ByVal v_lUserGroupID As Integer = 0) As Integer
        Dim result As Integer = 0
        Dim lReturn As Integer

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
                lReturn = g_oObjectManager.GetInstance(temp_oBusiness, "bPMWrkManager.FormClass", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                oBusiness = temp_oBusiness
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lReturn
                End If


                lReturn = oBusiness.GetUserAuthority(r_bIsAdministrator:=bIsAdministrator, r_vSupervisedGroups:=vSupervisedGroups)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

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
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetUserAuthorityFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUserAuthority", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function TaskStatusDescription(ByVal eTaskStatus As gPMConstants.PMEWrkManTaskStatus) As String

        Dim result As String = String.Empty
        Try

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

        Catch




            Return "Unknown"
        End Try

    End Function

    Private Sub SortColumn(ByVal Index As Integer)

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


                ' Sort by this column
                ListViewHelper.SetSortKeyProperty(lstScheduledTasks, Index - 1)
                ' Ascending
                ListViewHelper.SetSortOrderProperty(lstScheduledTasks, SortOrder.Ascending)

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

    Private Function PopulateListView() As Object
        Dim bInclude As Boolean
        Dim oListItem As ListViewItem
        Dim sType, sKey As String

        If Not Information.IsArray(m_vArray) Then
            Exit Function
        End If

        lstScheduledTasks.Items.Clear()

        ' Loop through tasks and add to listview
        For iCount As Integer = m_vArray.GetLowerBound(1) To m_vArray.GetUpperBound(1)

            bInclude = IncludeLine(vArray:=m_vArray, iCount:=iCount)

            If bInclude Then

                sKey = "Key" & CStr(m_vArray(0, iCount))

                If m_vArray(1, iCount) = gPMConstants.PMEReturnCode.PMTrue Then

                    oListItem = lstScheduledTasks.Items.Add(sKey, "Yes", "")
                Else
                    oListItem = lstScheduledTasks.Items.Add(sKey, "No", "")
                End If

                Select Case m_vArray(3, iCount)
                    Case gPMConstants.PMEWrkManTaskType.pmeWMTTMemo
                        sType = ACTaskTypeDescMemo
                    Case gPMConstants.PMEWrkManTaskType.pmeWMTTSingleComponent
                        sType = ACTaskTypeDescSingle
                    Case gPMConstants.PMEWrkManTaskType.pmeWMTTNavigatorProcess
                        sType = ACTaskTypeDescNavigator
                End Select

                ListViewHelper.GetListViewSubItem(oListItem, 1).Text = TaskStatusDescription(m_vArray(2, iCount))
                ListViewHelper.GetListViewSubItem(oListItem, 2).Text = CDate(m_vArray(5, iCount)).ToString("dd/MM/yyyy")
                ListViewHelper.GetListViewSubItem(oListItem, 3).Text = CStr(m_vArray(7, iCount))
                ListViewHelper.GetListViewSubItem(oListItem, 4).Text = CStr(m_vArray(6, iCount))
                ListViewHelper.GetListViewSubItem(oListItem, 5).Text = sType
                ListViewHelper.GetListViewSubItem(oListItem, 6).Text = CStr(m_vArray(8, iCount)).Trim()
                ListViewHelper.GetListViewSubItem(oListItem, 7).Text = CStr(m_vArray(9, iCount)).Trim()
                Dim TempDate As Date
                ListViewHelper.GetListViewSubItem(oListItem, 8).Text = IIf(DateTime.TryParse(CStr(m_vArray(5, iCount)), TempDate), TempDate.ToString("yyyyMMddHHMMss"), CStr(m_vArray(5, iCount)))
            End If

        Next iCount

    End Function

    Private Sub frmInterface_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed

        m_oNavigator.Dispose()
        'm_oComponent.Terminate

        m_oNavigator = Nothing
        'Set m_oComponent = Nothing

    End Sub

    Private Sub lstScheduledTasks_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lstScheduledTasks.ColumnClick
        Dim ColumnHeader As ColumnHeader = lstScheduledTasks.Columns(eventArgs.Column)

        If (ColumnHeader.Index + 1 - 1) = ACSTDueDateSortableCol Then
            ' We have used a hidden column for a sortable version of the
            ' due date. This code resets the column width to zero, if the
            ' user has found the column, expanded it and clicked in it.
            lstScheduledTasks.Columns.Item(ACSTDueDateSortableCol).Width = CInt(0)
            SortColumn(ACSTDueDateCol + 1)
        Else
            SortColumn(ColumnHeader.Index + 1)
        End If

    End Sub

    Public Function IncludeLine(ByVal vArray(,) As Object, ByVal iCount As Integer) As Boolean

        Dim result As Boolean = False
        Dim bNotInclude As Boolean
        Dim dtEndDate As Date

        result = True


        Dim strTaskStatus As String = TaskStatusDescription(vArray(2, iCount))

        If cboTaskStatus.SelectedIndex = 0 Then

            ' Check if we are looking at Not Complete tasks
            bNotInclude = Not (strTaskStatus = "New" Or strTaskStatus = "InComplete")

        Else

            ' Check if it is our selected task status
            bNotInclude = (strTaskStatus <> cboTaskStatus.Text) And (cboTaskStatus.SelectedIndex <> 1)

        End If

        If bNotInclude Then
            Return False
        End If

        If cboUser.ListIndex <> 0 Then
            bNotInclude = cboUser.ItemUsername.Trim() <> CStr(m_vArray(9, iCount)).Trim()
        End If

        If bNotInclude Then
            Return False
        End If

        If cboUserGroup.ListIndex <> 0 Then
            bNotInclude = cboUserGroup.ItemUserGroupname.Trim() <> CStr(m_vArray(8, iCount)).Trim()
        End If

        If bNotInclude Then
            Return False
        End If

        If cboDateRange.SelectedIndex > -1 Then
            ' Work out the Date Range End Date
            dtEndDate = DateTime.Now
            Select Case VB6.GetItemData(cboDateRange, cboDateRange.SelectedIndex)
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

            If dtEndDate <> #12/29/1899# Then
                bNotInclude = DateAndTime.DateDiff("d", CDate(CDate(m_vArray(5, iCount)).ToString("dd/MM/yyyy")), dtEndDate, FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) <= 0
            End If

            If bNotInclude Then
                Return False
            End If

        End If

        Return result
    End Function

    Private Function ReAssign(Optional ByRef bEdit As Boolean = False) As Integer

        'Modified by Sumeet Singh on 5/19/2010 7:24:31 PM to do list (Iteration 3)
        'Dim oWorkManager As iPMWrkTaskInstance.Interface_Renamed
        Dim oWorkManager As Object
        Dim lCombos(3) As Integer

        ' Hold combo and row information
        Dim lRow As Integer = lstScheduledTasks.FocusedItem.Index + 1
        lCombos(0) = cboTaskStatus.SelectedIndex
        lCombos(1) = cboUserGroup.ListIndex
        lCombos(2) = cboUser.ListIndex
        lCombos(3) = cboDateRange.SelectedIndex

        Dim temp_oWorkManager As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_oWorkManager, sClassName:="iPMWrkTaskInstance.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
        oWorkManager = temp_oWorkManager

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            oWorkManager.Dispose()
            oWorkManager = Nothing
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_lReturn = oWorkManager.Initialise

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            oWorkManager.Dispose()
            oWorkManager = Nothing
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If bEdit Then

            m_lReturn = oWorkManager.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit, vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired, vEffectiveDate:=DateTime.Now)

        Else


            m_lReturn = oWorkManager.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMDelete, vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired, vEffectiveDate:=DateTime.Now)
        End If

        'Set the ID of the current attached task.

        oWorkManager.PMWrkTaskInstanceCnt = m_lTaskID

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            oWorkManager = Nothing
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Start the Form

        m_lReturn = oWorkManager.Start

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            oWorkManager.Dispose()
            oWorkManager = Nothing
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        oWorkManager.Dispose()
        oWorkManager = Nothing
        m_lReturn = GetTaskList(bSetup:=False)

        ' Reset the combo boxes to what they were before we refreshed
        cboTaskStatus.SelectedIndex = lCombos(0)
        cboUserGroup.ListIndex = lCombos(1)
        cboUser.ListIndex = lCombos(2)
        cboDateRange.SelectedIndex = lCombos(3)

        ' Reselect the row we were looking at
        If lstScheduledTasks.Items.Count > 0 Then
            lstScheduledTasks.Items.Item(lRow - 1).Selected = True
        End If

        Return gPMConstants.PMEReturnCode.PMTrue

    End Function

    Private Function View() As Integer
        Dim result As Integer = 0
        Dim oWorkManager As iPMWrkTaskInstance.Interface_Renamed
        Try


            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
            result = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_oWorkManager As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oWorkManager, sClassName:="iPMWrkTaskInstance.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oWorkManager = temp_oWorkManager

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                oWorkManager.Dispose()
                oWorkManager = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = oWorkManager.Initialise

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                oWorkManager.Dispose()
                oWorkManager = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = oWorkManager.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMView, vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired, vEffectiveDate:=DateTime.Now)

            'Set the ID of the current attached task.

            oWorkManager.PMWrkTaskInstanceCnt = m_lTaskID

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                oWorkManager = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Start the Form

            m_lReturn = oWorkManager.Start

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                oWorkManager.Dispose()
                oWorkManager = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            oWorkManager.Dispose()
            oWorkManager = Nothing
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="View Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="View", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function

    Private Sub lstScheduledTasks_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lstScheduledTasks.DoubleClick

        Dim bAuthorised As Boolean
        Dim lPMAuthorityLevel, lPMUserGroupID As Integer
        Dim bIsUsersGroup As Boolean

        If lstScheduledTasks.FocusedItem Is Nothing Then
            Exit Sub
        End If
        'Developer Guide No. 52
        If lstScheduledTasks.FocusedItem.SubItems(1).Text.Length > 0 Then

            'Get the User GroupID for this Task
            For lCount As Integer = 0 To m_vArray.GetUpperBound(1)
                If CDbl(m_vArray(0, lCount)) = CInt(Mid(lstScheduledTasks.FocusedItem.Name, 4)) Then
                    lPMUserGroupID = CInt(CStr(m_vArray(19, lCount)).Trim())
                    Exit For
                End If
            Next

            'Get user authority for this user group.
            m_lReturn = GetUserAuthority(r_lPMAuthorityLevel:=lPMAuthorityLevel, v_lUserGroupID:=lPMUserGroupID)

            'Allow access to task if user is a system administrator or a superviser for this user group.
            bAuthorised = lPMAuthorityLevel <> 0

            If Not bAuthorised Then
                'Developer Guide No. 52
                If lstScheduledTasks.FocusedItem.SubItems(ACSTUserCol).Text.Trim().ToUpper() = g_sUsername.Trim().ToUpper() Then
                    'Always authorised to work on own tasks.
                    bAuthorised = True
                Else
                    'Is this group one that the user normally has access to.
                    m_lReturn = IsUsersGroup(v_lUserGroupID:=lPMUserGroupID, r_bIsUsersGroup:=bIsUsersGroup)
                    'Developer Guide No. 52
                    If bIsUsersGroup And lstScheduledTasks.FocusedItem.SubItems(ACSTUserCol).Text = "" Then
                        'If this is an unassigned task for a group the user has access to then the user is authorised to use it.
                        bAuthorised = True
                    End If
                End If
            End If
            'Developer Guide No. 52
            If lstScheduledTasks.FocusedItem.SubItems(1).Text <> "In Progress" And lstScheduledTasks.FocusedItem.SubItems(1).Text <> "Complete" And bAuthorised Then
                mnuStart_Click(mnuStart, New EventArgs())
            End If
        End If

    End Sub

    Private Sub lstScheduledTasks_KeyUp(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles lstScheduledTasks.KeyUp
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000
        Try

            Dim bAuthorised As Boolean
            Dim lPMAuthorityLevel, lPMUserGroupID As Integer
            Dim bIsUsersGroup As Boolean

            If lstScheduledTasks.FocusedItem Is Nothing Then
                Exit Sub
            End If

            'Developer Guide No. 52
            If lstScheduledTasks.FocusedItem.SubItems(1).Text.Length > 0 Then

                'Get the User GroupID for this Task
                For lCount As Integer = 0 To m_vArray.GetUpperBound(1)
                    If CDbl(m_vArray(0, lCount)) = CInt(Mid(lstScheduledTasks.FocusedItem.Name, 4)) Then
                        lPMUserGroupID = CInt(CStr(m_vArray(19, lCount)).Trim())
                        Exit For
                    End If
                Next

                'Get user authority for this user group.
                m_lReturn = GetUserAuthority(r_lPMAuthorityLevel:=lPMAuthorityLevel, v_lUserGroupID:=lPMUserGroupID)

                'Allow access to task if user is a system administrator or a superviser for this user group.
                bAuthorised = lPMAuthorityLevel <> 0

                If Not bAuthorised Then
                    'Developer Guide No. 52
                    If lstScheduledTasks.FocusedItem.SubItems(ACSTUserCol).Text.Trim().ToUpper() = g_sUsername.Trim().ToUpper() Then
                        'Always authorised to work on own tasks.
                        bAuthorised = True
                    Else
                        'Is this group one that the user normally has access to.
                        m_lReturn = IsUsersGroup(v_lUserGroupID:=lPMUserGroupID, r_bIsUsersGroup:=bIsUsersGroup)
                        'Developer Guide No. 52
                        If bIsUsersGroup And lstScheduledTasks.FocusedItem.SubItems(ACSTUserCol).Text = "" Then
                            'If this is an unassigned task for a group the user has access to then the user is authorised to use it.
                            bAuthorised = True
                        End If
                    End If
                End If

                'Check which key was pressed
                If eventArgs.KeyCode = Keys.A And (Shift = ShiftConstants.CtrlMask) Then
                    'Developer Guide No. 52
                    If lstScheduledTasks.FocusedItem.SubItems(1).Text <> "In Progress" And lstScheduledTasks.FocusedItem.SubItems(1).Text <> "Complete" And bAuthorised Then
                        mnuAssign_Click(mnuAssign, New EventArgs())
                    End If
                End If

                If eventArgs.KeyCode = Keys.E And (Shift = ShiftConstants.CtrlMask) Then
                    'Developer Guide No. 52
                    If lstScheduledTasks.FocusedItem.SubItems(1).Text <> "In Progress" And lstScheduledTasks.FocusedItem.SubItems(1).Text <> "Complete" And bAuthorised Then
                        mnuEdit_Click(mnuEdit, New EventArgs())
                    End If
                End If

                If eventArgs.KeyCode = Keys.V And (Shift = ShiftConstants.CtrlMask) Then
                    mnuView_Click(mnuView, New EventArgs())
                End If

                If eventArgs.KeyCode = Keys.L And (Shift = ShiftConstants.CtrlMask) Then
                    m_lReturn = ViewTaskLog()
                End If

                If eventArgs.KeyCode = Keys.S And (Shift = ShiftConstants.CtrlMask) Then
                    'Developer Guide No. 52
                    If lstScheduledTasks.FocusedItem.SubItems(1).Text <> "In Progress" And lstScheduledTasks.FocusedItem.SubItems(1).Text <> "Complete" And bAuthorised Then
                        mnuStart_Click(mnuStart, New EventArgs())
                    End If
                End If

                If eventArgs.KeyCode = Keys.C And (Shift = ShiftConstants.CtrlMask) Then
                    ' Check that we can complete this task
                    'Developer Guide No. 52
                    If (lstScheduledTasks.FocusedItem.SubItems(1).Text = "New" Or lstScheduledTasks.FocusedItem.SubItems(1).Text = "InComplete") And bAuthorised Then
                        mnuComplete_Click(mnuComplete, New EventArgs())
                    End If
                End If

                If eventArgs.KeyCode = Keys.I And (Shift = ShiftConstants.CtrlMask) Then
                    ' Check that we can set this task to incomplete
                    'Developer Guide No. 52
                    If (lstScheduledTasks.FocusedItem.SubItems(1).Text = "In Progress" Or lstScheduledTasks.FocusedItem.SubItems(1).Text = "Complete") And bAuthorised Then
                        mnuInComplete_Click(mnuInComplete, New EventArgs())
                    End If
                End If
            End If

            'DC080304 PN10831 Exit Sub was missing

        Catch excep As System.Exception


            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="lstScheduledTasks_KeyUp Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="lstScheduledTasks_KeyUp", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub


        End Try
    End Sub

    Private Sub lstScheduledTasks_MouseUp(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lstScheduledTasks.MouseUp
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
        Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
        Dim bAuthorised As Boolean
        Dim lPMAuthorityLevel, lPMUserGroupID As Integer
        Dim bIsUsersGroup As Boolean

        ' If Right Mouse Click
        'Developer Guide No. 64
        'If Button = MouseButtonConstants.RightButton Then
        If Button = MouseButtons.Right Then

            If lstScheduledTasks.FocusedItem Is Nothing Then
                Exit Sub
            End If

            ' Check to see if Clicked on a Scheduled Task
            'Developer Guide No. 52
            If lstScheduledTasks.FocusedItem.SubItems(1).Text.Length > 0 Then

                'Get the User GroupID for this Task
                For lCount As Integer = 0 To m_vArray.GetUpperBound(1)
                    If CDbl(m_vArray(0, lCount)) = CInt(Mid(lstScheduledTasks.FocusedItem.Name, 4)) Then
                        lPMUserGroupID = CInt(CStr(m_vArray(19, lCount)).Trim())
                        Exit For
                    End If
                Next

                'Get user authority for this user group.
                m_lReturn = GetUserAuthority(r_lPMAuthorityLevel:=lPMAuthorityLevel, v_lUserGroupID:=lPMUserGroupID)

                'Allow access to task if user is a system administrator or a superviser for this user group.
                bAuthorised = lPMAuthorityLevel <> 0

                If Not bAuthorised Then
                    'Developer Guide No. 52
                    If lstScheduledTasks.FocusedItem.SubItems(ACSTUserCol).Text.Trim().ToUpper() = g_sUsername.Trim().ToUpper() Then
                        'Always authorised to work on own tasks.
                        bAuthorised = True
                    Else
                        'Is this group one that the user normally has access to.
                        m_lReturn = IsUsersGroup(v_lUserGroupID:=lPMUserGroupID, r_bIsUsersGroup:=bIsUsersGroup)
                        'Developer Guide No. 52
                        If bIsUsersGroup And lstScheduledTasks.FocusedItem.SubItems(ACSTUserCol).Text = "" Then
                            'If this is an unassigned task for a group the user has access to then the user is authorised to use it.
                            bAuthorised = True
                        End If
                    End If
                End If

                ' Set menu state according to user authority and type of task
                'Developer Guide No. 52
                If lstScheduledTasks.FocusedItem.SubItems(1).Text <> "In Progress" And lstScheduledTasks.FocusedItem.SubItems(1).Text <> "Complete" And bAuthorised Then
                    mnuAssign.Enabled = True
                    mnuEdit.Enabled = True
                    mnuStart.Enabled = True
                Else
                    mnuAssign.Enabled = False
                    mnuEdit.Enabled = False
                    mnuStart.Enabled = False
                End If
                'Developer Guide No. 52
                mnuComplete.Enabled = (lstScheduledTasks.FocusedItem.SubItems(1).Text = "New" Or lstScheduledTasks.FocusedItem.SubItems(1).Text = "InComplete") And bAuthorised
                'Developer Guide No. 52
                mnuInComplete.Enabled = (lstScheduledTasks.FocusedItem.SubItems(1).Text = "In Progress" Or lstScheduledTasks.FocusedItem.SubItems(1).Text = "Complete") And bAuthorised

                Ctx_mnuTask.Show(Me, PointToClient(Cursor.Position).X, PointToClient(Cursor.Position).Y)
            End If
        End If

    End Sub

    Private Sub lstScheduledTasks_ItemClick(ByVal Item As ListViewItem)

        If Item Is Nothing Then
            Exit Sub
        Else
            m_lTaskID = CInt(Item.Name.Substring(3, Math.Min(Item.Name.Length, Strings.Len(Item.Name) - 3)))
        End If

    End Sub
    Private Sub lstScheduledTasks_ItemSelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ListViewItemSelectionChangedEventArgs) Handles lstScheduledTasks.ItemSelectionChanged
        lstScheduledTasks_ItemClick(e.Item)
    End Sub

    Public Function StartScheduledTask(ByVal v_lPMWrkTaskInstanceCnt As Integer) As Integer
        Dim result As Integer = 0
        Dim oBusiness As bPMWrkManager.FormClass
        Dim lReturn, lPMAuthorityLevel, lPMUserGroupID, iCount As Integer
        Dim vSetKeyArray As Object
        Dim strProcess As String = ""
        Dim lPMNavProcessID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the User GroupID for this Task
            For iCount = 0 To m_vArray.GetUpperBound(1)
                If CDbl(m_vArray(0, iCount)) = v_lPMWrkTaskInstanceCnt Then
                    lPMUserGroupID = CInt(CStr(m_vArray(19, iCount)).Trim())
                    Exit For
                End If
            Next iCount

            ' Get the Business
            Dim temp_oBusiness As Object
            lReturn = g_oObjectManager.GetInstance(temp_oBusiness, "bPMWrkManager.FormClass", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oBusiness = temp_oBusiness
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return lReturn
            End If

            ' Work out the User's Authority Level for this Task
            lReturn = GetUserAuthority(r_lPMAuthorityLevel:=lPMAuthorityLevel, v_lUserGroupID:=lPMUserGroupID)


            Select Case m_vArray(3, iCount)
                Case gPMConstants.PMEWrkManTaskType.pmeWMTTMemo

                    lReturn = MessageBox.Show("Customer: " & CStr(m_vArray(6, iCount)) & Strings.Chr(13).ToString() & Strings.Chr(10).ToString() & _
                              Strings.Chr(13).ToString() & Strings.Chr(10).ToString() & _
                              "Description: " & CStr(m_vArray(7, iCount)) & _
                              Strings.Chr(13).ToString() & Strings.Chr(10).ToString() & Strings.Chr(13).ToString() & Strings.Chr(10).ToString() & _
                              "Has This Task Been Completed?", Application.ProductName, MessageBoxButtons.YesNo)

                    If lReturn = System.Windows.Forms.DialogResult.Yes Then
                        ' Mark as complete
                        lReturn = CompleteSchedTask(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt)
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                    ElseIf lReturn = System.Windows.Forms.DialogResult.No Then
                        ' Mark as incomplete
                        lReturn = IncompleteSchedTask(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt)
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                    Else
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                Case gPMConstants.PMEWrkManTaskType.pmeWMTTNavigatorProcess

                    'm_oNavigator.Initialise True

                    ' Set the task InProgress

                    lReturn = oBusiness.SetStatusInProgress(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt)

                    ' Get the Set Keys for this Task Instance

                    lReturn = oBusiness.GetTaskInstKeys(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt, r_vKeyArray:=vSetKeyArray)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' If we have an XMLFile value it will be used over and above any value
                    ' for ProcessID which will probably be "" anyway. To prevent errors set
                    ' ProcessID to dummy value if not set when XMLfile is   PN14908
                    If CStr(m_vArray(20, iCount)).Trim() <> "" And CStr(m_vArray(10, iCount)).Trim() = "" Then
                        lPMNavProcessID = 0
                    Else
                        lPMNavProcessID = CInt(m_vArray(10, iCount))
                    End If

                    ' Start the task

                    lReturn = StartNavigatorTask(v_lPMNavProcessID:=lPMNavProcessID, v_lPMAuthorityLevel:=lPMAuthorityLevel, v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt, v_vSetKeyArray:=vSetKeyArray, v_sNavXMLfile:=CStr(m_vArray(20, iCount)).Trim())
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                Case gPMConstants.PMEWrkManTaskType.pmeWMTTSingleComponent

                    'm_oComponent.Initialise

                    strProcess = CStr(m_vArray(11, iCount)) & "." & CStr(m_vArray(12, iCount))

                    ' Stop users trying to display any Client Manager tasks. Could cause all sorts of
                    ' problems. Especially as they are probably already in the client/policy.
                    If CStr(m_vArray(11, iCount)) = "iPMBClientTaskWrapper" Or CStr(m_vArray(11, iCount)) = "iPMBClientManagerWrapper" Then
                        lReturn = MessageBox.Show("You cannot run this type of task from within Client Manager" & Strings.Chr(13) & Strings.Chr(10) & "Please start this task from Work Manager", "Task List", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                        Return result
                    End If

                    m_oNavigator.Initialise(False)

                    ' Set the task InProgress

                    lReturn = oBusiness.SetStatusInProgress(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt)

                    ' Get the Set Keys for this Task Instance

                    lReturn = oBusiness.GetTaskInstKeys(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt, r_vKeyArray:=vSetKeyArray)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    '            ' If there are any Set Keys then Set them
                    '            If (IsArray(vSetKeyArray) = True) Then
                    '                lReturn = m_oComponent.StartComponent( _
                    ''                    v_sComponent:=strProcess, _
                    ''                    v_lPMAuthorityLevel:=lPMAuthorityLevel, _
                    ''                    v_vSetKeyArray:=vSetKeyArray)
                    '            Else
                    '                lReturn = m_oComponent.StartComponent( _
                    ''                    v_sComponent:=strProcess, _
                    ''                    v_lPMAuthorityLevel:=lPMAuthorityLevel)
                    '            End If
                    '
                    ' If there are any Set Keys then Set them
                    If Information.IsArray(vSetKeyArray) Then

                        lReturn = m_oNavigator.Component.StartComponent(v_sComponent:=strProcess, v_lPMAuthorityLevel:=lPMAuthorityLevel, v_vSetKeyArray:=CStr(vSetKeyArray))
                    Else
                        lReturn = m_oNavigator.Component.StartComponent(v_sComponent:=strProcess, v_lPMAuthorityLevel:=lPMAuthorityLevel)
                    End If

                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                        oDict.Add("v_lPMWrkTaskInstanceCnt", v_lPMWrkTaskInstanceCnt)
                        gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Start Component :- " & strProcess, oDicParms:=oDict)
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    lReturn = CompleteSchedTask(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                        oDict.Add("v_lPMWrkTaskInstanceCnt", v_lPMWrkTaskInstanceCnt)
                        gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Complete Task.", vApp:=ACApp, vClass:=ACClass, vMethod:="StartScheduledTask", oDicParms:=oDict)
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

            End Select

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="StartScheduledTaskFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="StartScheduledTask", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function

    'Private Sub m_oNavigator_NavigatorClose()
    '
    '    m_oNavigator.Terminate
    '
    'End Sub

    Public Sub mnuAssign_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuAssign.Click

        m_lReturn = ReAssign()

    End Sub

    Public Sub mnuComplete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuComplete.Click

        ' Complete
        m_lReturn = CompleteSchedTask(v_lPMWrkTaskInstanceCnt:=m_lTaskID)

    End Sub

    Public Sub mnuEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuEdit.Click

        m_lReturn = ReAssign(bEdit:=True)

    End Sub

    Public Sub mnuInComplete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuInComplete.Click

        ' Incomplete
        m_lReturn = IncompleteSchedTask(v_lPMWrkTaskInstanceCnt:=m_lTaskID)

    End Sub

    Public Sub mnuStart_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuStart.Click

        m_lReturn = StartScheduledTask(v_lPMWrkTaskInstanceCnt:=m_lTaskID)

    End Sub

    Public Sub mnuView_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuView.Click

        m_lReturn = View()

    End Sub

    'DC180804 PN14282 added TaskLog as option
    Public Sub mnuTaskLog_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuTaskLog.Click

        m_lReturn = ViewTaskLog()

    End Sub


    ' ***************************************************************** '
    ' Name: IncompleteSchedTask
    '
    ' Description: Marks the Task As InCompleted.
    '
    ' ***************************************************************** '
    Private Function IncompleteSchedTask(ByVal v_lPMWrkTaskInstanceCnt As Integer) As Integer
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        Dim oBusiness As bPMWrkManager.FormClass

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create instance of business object
            Dim temp_oBusiness As Object
            lReturn = g_oObjectManager.GetInstance(temp_oBusiness, "bPMWrkManager.FormClass", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oBusiness = temp_oBusiness

            ' InComplete the Task

            lReturn = oBusiness.SetStatusInComplete(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to Incomplete Task")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = GetTaskList(bSetup:=False)

            oBusiness = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="IncompleteSchedTaskFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="IncompleteSchedTask", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CompleteSchedTask
    '
    ' Description: Marks the Task As Completed.
    '
    ' ***************************************************************** '
    Private Function CompleteSchedTask(ByVal v_lPMWrkTaskInstanceCnt As Integer) As Integer
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        Dim oBusiness As bPMWrkManager.FormClass
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create instance of business object
            Dim temp_oBusiness As Object
            lReturn = g_oObjectManager.GetInstance(temp_oBusiness, "bPMWrkManager.FormClass", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oBusiness = temp_oBusiness

            ' Complete the Task

            lReturn = oBusiness.SetStatusComplete(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to Complete Task")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = GetTaskList(bSetup:=False)

            oBusiness = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CompleteSchedTaskFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="CompleteSchedTask", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: StartNavigatorTask
    '
    ' Description: Starts a Navigator Task
    '
    ' ***************************************************************** '
    'Developer Guide No. 33
    Public Function StartNavigatorTask(ByVal v_lPMNavProcessID As Integer, ByVal v_lPMAuthorityLevel As Integer, ByVal v_sNavXMLfile As String, Optional ByVal v_vSetKeyArray(,) As Object = Nothing, Optional ByVal v_lPMWrkTaskInstanceCnt As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim lReturn As Integer
        Dim oNav As Object
        Dim iIndex As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'DJM 18/02/2004 : Add XML Navigator functionality.
            m_oNavigator.NavXMLfile = v_sNavXMLfile

            ' Add a New In Progress Task
            m_oNavigator.Initialise(True)

            If v_sNavXMLfile.Trim() <> "" Then

                oNav = m_oNavigator.NavigatorXM

                ' Set the Authority Level

                oNav.NavigatorV3_PMAuthorityLevel = v_lPMAuthorityLevel


                oNav.XMLFileName = v_sNavXMLfile

            Else

                oNav = m_oNavigator.Navigator

                ' Set the Process ID and Authority Level

                oNav.ProcessID = v_lPMNavProcessID

                oNav.PMAuthorityLevel = v_lPMAuthorityLevel

            End If

            'With m_oNavigator
            With oNav

                If Information.IsArray(v_vSetKeyArray) Then
                    iIndex = v_vSetKeyArray.GetUpperBound(1)
                    ReDim Preserve v_vSetKeyArray(1, iIndex)
                Else
                    iIndex = 0
                    ReDim v_vSetKeyArray(1, iIndex)
                End If


                v_vSetKeyArray(0, iIndex) = "TaskInstanceCnt"

                v_vSetKeyArray(1, iIndex) = v_lPMWrkTaskInstanceCnt

                ' If there are any Set Keys then Set them
                If Information.IsArray(v_vSetKeyArray) Then

                    lReturn = .SetKeys(v_vSetKeyArray)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                        oDict.Add("v_lPMNavProcessID", v_lPMNavProcessID)
                        oDict.Add("v_lPMWrkTaskInstanceCnt", v_lPMWrkTaskInstanceCnt)
                        gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Navigator Set Keys Failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="StartNavigatorTask", oDicParms:=oDict)
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If

                ' Tell Navigator to Start

                lReturn = .Start
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                    oDict.Add("v_lPMNavProcessID", v_lPMNavProcessID)
                    oDict.Add("v_lPMWrkTaskInstanceCnt", v_lPMWrkTaskInstanceCnt)
                    gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Start Navigator.", vApp:=ACApp, vClass:=ACClass, vMethod:="StartNavigatorTask", oDicParms:=oDict)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            lReturn = CompleteSchedTask(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("v_lPMNavProcessID", v_lPMNavProcessID)
                oDict.Add("v_lPMWrkTaskInstanceCnt", v_lPMWrkTaskInstanceCnt)
                gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Complete Task.", vApp:=ACApp, vClass:=ACClass, vMethod:="StartNavigatorTask", oDicParms:=oDict)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_lPMNavProcessID", v_lPMNavProcessID)
            oDict.Add("v_lPMWrkTaskInstanceCnt", v_lPMWrkTaskInstanceCnt)
            gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="StartNavigatorTaskFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="StartNavigatorTask", excep:=excep, oDicParms:=oDict)

            Return result



            Return result
        End Try
    End Function

    Private Function ViewTaskLog() As Integer
        Dim result As Integer = 0

        'Modified by Sumeet Singh on 5/19/2010 7:23:39 PM to do list (Iteration 3)
        'Dim oTaskLog As iPMWrkTaskInstLog.Interface_Renamed
        Dim oTaskLog As Object
        Try


            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
            Dim temp_oTaskLog As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oTaskLog, sClassName:="iPMWrkTaskInstLog.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oTaskLog = temp_oTaskLog
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return m_lReturn
            End If


            m_lReturn = oTaskLog.Initialise
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                oTaskLog.Dispose()
                oTaskLog = Nothing
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return m_lReturn
            End If


            m_lReturn = oTaskLog.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit, vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired, vEffectiveDate:=DateTime.Now)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                oTaskLog.Dispose()
                oTaskLog = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Set the ID of the current attached task.

            oTaskLog.PMWrkTaskInstanceCnt = m_lTaskID

            oTaskLog.Start()


            oTaskLog.Dispose()
            oTaskLog = Nothing
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ViewTaskLog Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ViewTaskLog", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Return result
        End Try
    End Function

    Public Function IsUsersGroup(ByVal v_lUserGroupID As Integer, ByRef r_bIsUsersGroup As Boolean) As Integer
        Dim result As Integer = 0
        Dim oBusiness As bPMUserGroup.Business
        Static vUsersGroups As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            r_bIsUsersGroup = False


            If Object.Equals(vUsersGroups, Nothing) Then

                ' Get the Business
                Dim temp_oBusiness As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_oBusiness, "bPMUserGroup.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                oBusiness = temp_oBusiness
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If


                m_lReturn = oBusiness.GetUserGroupInfo(r_lUserId:=g_oObjectManager.UserID, r_vUserGroupInfo:=vUsersGroups)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    oBusiness.Dispose()
                    oBusiness = Nothing
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                oBusiness.Dispose()
                oBusiness = Nothing

            End If

            If Information.IsArray(vUsersGroups) Then

                For lRow As Integer = vUsersGroups.GetLowerBound(1) To vUsersGroups.GetUpperBound(1)


                    If v_lUserGroupID = CInt(vUsersGroups(0, lRow)) And CInt(vUsersGroups(3, lRow)) <> 0 Then
                        r_bIsUsersGroup = True
                        Exit For
                    End If
                Next lRow
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="IsUsersGroupFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="IsUsersGroup", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


End Class