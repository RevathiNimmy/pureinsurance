Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Globalization
Imports System.Windows.Forms
'Developer Guide No.129
Imports SharedFiles
Partial Friend Class frmAssignMultipleTask
    Inherits System.Windows.Forms.Form

    ' ***************************************************************** '
    ' Form Name: frmAssignMultipleTask
    '
    ' Description: Main interface for Multi Task Assignment Form
    '
    ' Edit History:
    ' RAM20020715 : Created
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmAssignMultipleTask"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' {* GENERATED CODE (Begin) *}

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As gPMConstants.PMEReturnCode
    Private m_lErrorNumber As gPMConstants.PMEReturnCode

    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_lPmuserGroupID As Integer
    Private m_iUserID As Integer
    ' RDC 16082002
    'Private m_lUserGroupID As Long

    Private m_bFormLoaded As Boolean
    Private m_bFormShown As Boolean

    Private m_bDirty As Boolean

    ' RDC 16082002 for group user array
    Private Const GROUP_USER_ID As Integer = 0
    Private Const GROUP_USER_NAME As Integer = 1

    ' {* USER DEFINED CODE (End) *}

    ' Declare an instance of the Business object.
    'Private m_oBusiness As bPMWrkTaskInstance.FormClass

    Private m_oBusiness As bPMWrkTaskInstance.FormClass

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Array to hold all the TaskInstanceCnt
    Private m_vPMWrkTaskInstanceCntArray() As Object

    ' {* USER DEFINED CODE (Begin) *}
    ' {* USER DEFINED CODE (End) *}
    ' PRIVATE Data Members (End)

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


    ' {* USER DEFINED CODE (End) *}

    ' PUBLIC Property Procedures (End)

    ' PRIVATE Property Procedures (Begin)

    'UPGRADE_NOTE: (7001) The following declaration (let Status) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub Status(ByVal Value As Integer)
    '
    ' Standard Property.
    '
    ' Set the interface exit status.
    'm_lStatus = Value
    '
    'End Sub
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

    Public Property PMWrkTaskInstanceCntArray() As Object
        Get
            Return VB6.CopyArray(m_vPMWrkTaskInstanceCntArray)
        End Get
        Set(ByVal Value As Object)
            m_vPMWrkTaskInstanceCntArray = Value
        End Set
    End Property

    Public Property UserID() As Integer
        Get
            Return m_iUserID
        End Get
        Set(ByVal Value As Integer)
            m_iUserID = Value
        End Set
    End Property

    Public Property UserGroupID() As Integer
        Get
            Return m_lPmuserGroupID
        End Get
        Set(ByVal Value As Integer)
            m_lPmuserGroupID = Value
        End Set
    End Property

    ' RDC 10092002
    Public ReadOnly Property FormShown() As Boolean
        Get
            Return m_bFormShown
        End Get
    End Property
    ' PRIVATE Property Procedures (End)


    ' PUBLIC Methods (Begin)
    Public Function Initialise() As Integer

        Dim result As Integer = 0
        Dim sMessage, sTitle As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bPMWrkTaskInstance.FormClass", vinstancemanager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                sTitle = ACBusinessFailTitleText
                sMessage = ACBusinessFailText

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                Return result
            End If

            ' Set the interface status to cancelled. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            m_lReturn = CType(Load_Renamed(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Return result

        Catch excep As System.Exception



            ' Error Section
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function Load_Renamed() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Set the interface default values.
            m_lReturn = CType(SetInterfaceDefaults(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return result
            End If

            ' Gets the interface details to be displayed.
            m_lReturn = CType(GetInterfaceDetails(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the interface details.
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return result
            End If

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Return result

        Catch excep As System.Exception



            ' Error Section
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetInterfaceDefaults
    '
    ' Description: Sets all of the interface default values.
    '
    ' DAK070999 - Add null option to cboDueDate
    ' ***************************************************************** '
    Private Function SetInterfaceDefaults() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.
            iPMFunc.CenterForm(Me)

            ' Set any other default values to the interface.

            ' {* USER DEFINED CODE (Begin) *}


            Select Case Task
                ' Add a New Task Instance
                Case gPMConstants.PMEComponentAction.PMAdd

                    ' Edit the Details of an Existing Task Instance
                Case gPMConstants.PMEComponentAction.PMEdit

                    ' View an existing Task Instance
                Case gPMConstants.PMEComponentAction.PMView

                    ' ReAssign a Task Instance
                Case Else

                    '        cboTaskUserGroup.Enabled = True
                    '        cboTaskUser.Enabled = True

            End Select

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

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

            m_lReturn = CType(BusinessToInterface(), gPMConstants.PMEReturnCode)

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



    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: BusinessToInterface
    '
    ' Description: Updates all interface details from the business
    '              object.
    '
    ' AMB 21/01/2003 - Added workflow_information field - IAG Spec 229
    ' ***************************************************************** '
    Private Function BusinessToInterface() As Integer

        Dim result As Integer = 0
        Dim lPmwrkTaskInstanceCnt As Integer
        Dim lReturn As gPMConstants.PMEReturnCode

        Dim lPMWrkTaskGroupId, lPMWrkTaskId As Integer
        Dim sCustomer As String = ""
        Dim dtTaskDueDate As Date
        Dim lPMUserGroupID As Integer
        Dim iUserID As Integer
        Dim sDescription As String = ""
        Dim iTaskStatus As gPMConstants.PMEWrkManTaskStatus
        Dim iIsUrgent As gPMConstants.PMEReturnCode
        Dim dtDateCreated As Date
        Dim iCreatedByID As Integer
        Dim dtLastModified As Date
        Dim iModifiedByID As Integer
        ' AMB 21/01/2003
        Dim sWorkflowInformation As String = ""
        Dim sTaskTypeDesc, sTaskStatusDesc, sUserGroup, sUser As String
        ' RDC 06092002
        Dim iSprVsrCount, iIsSupervisor As Integer
        Dim vUserGroups As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the interface details.

            ' RDC 10092002
            iSprVsrCount = 0

            m_bFormLoaded = False

            ' RDC 06092002 preload status bar message
            barStatus.Items.Item(0).Text = "Information: User is able to re-assign all selected tasks"

            lstScheduledTasks.Items.Clear()

            If Information.IsArray(m_vPMWrkTaskInstanceCntArray) Then

                For iCounter As Integer = m_vPMWrkTaskInstanceCntArray.GetLowerBound(0) To m_vPMWrkTaskInstanceCntArray.GetUpperBound(0)

                    lPmwrkTaskInstanceCnt = CInt(m_vPMWrkTaskInstanceCntArray(iCounter))


                    m_lReturn = m_oBusiness.GetDetails(v_lPMWrkTaskInstanceCnt:=lPmwrkTaskInstanceCnt, r_lPMWrkTaskGroupID:=lPMWrkTaskGroupId, r_lpmwrktaskid:=lPMWrkTaskId, r_scustomer:=sCustomer, r_dttaskduedate:=dtTaskDueDate, r_lpmusergroupid:=lPMUserGroupID, r_iuserid:=iUserID, r_sdescription:=sDescription, r_itaskstatus:=iTaskStatus, r_iisurgent:=iIsUrgent, r_dtdatecreated:=dtDateCreated, r_icreatedbyid:=iCreatedByID, r_dtlastmodified:=dtLastModified, r_imodifiedbyid:=iModifiedByID, r_sWorkflowInformation:=sWorkflowInformation)
                    ' AMB 21/01/2003

                    ' Check for errors.
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        ' Log Error.
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
                    End If

                    ' Get the User Name
                    Dim dbNumericTemp As Double
                    If Double.TryParse(CStr(iUserID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                        If iUserID < 1 Then
                            sUser = ""
                        Else
                            sUser = cboAllUsers.ItemUsername(iUserID)
                        End If
                    Else
                        sUser = ""
                    End If

                    ' RDC 06092002 check that user is a supervisor on the task group's user group
                    m_lReturn = CType(CheckIsSupervisor(m_iUserID, lPMUserGroupID, iIsSupervisor), gPMConstants.PMEReturnCode)

                    ' Check for errors.
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        ' Log Error.
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to check if user is supervisor of task group's user group", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
                    End If

                    ' Work Out the Task Status Description
                    sTaskStatusDesc = TaskStatusDescription(iTaskStatus)

                    ' RDC 06092002 only add the task to the list if user is supervisor
                    ' for the group that the task is assigned to
                    If iIsSupervisor = 0 Then
                        barStatus.Items.Item(0).Text = "Warning: Tasks displayed are those that user has supervisor access to"
                    Else
                        iSprVsrCount += 1

                        ' add to list of user groups that can perform this task

                        lReturn = CType(GetTaskUserGroups(lPMWrkTaskId, vUserGroups), gPMConstants.PMEReturnCode)

                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' it failed
                            result = gPMConstants.PMEReturnCode.PMFalse

                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get user groups for task", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")

                            Return result

                        End If

                        ' add the task to the form
                        lReturn = CType(AddScheduledTaskToList("ST" & lPmwrkTaskInstanceCnt, iIsUrgent, sTaskStatusDesc, sTaskTypeDesc, dtTaskDueDate, sCustomer, sDescription, sUserGroup, sUser, sWorkflowInformation), gPMConstants.PMEReturnCode)
                    End If

                Next iCounter

                ' Assign the details to the interface.
                'RefreshUserList v_iUserId:=iUserID

            End If

            If Not Information.IsArray(vUserGroups) Then
                ' this collection of tasks has no common user groups

                ' force the form
                Me.Show()

                ' set flag so that Interface.ProcessInterface doesn't try to show it again
                m_bFormShown = True

                ' don't want the user to hit OK
                cmdOK.Enabled = False

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                ' tell them the bad news
                If iSprVsrCount > 0 Then

                    barStatus.Items.Item(0).Text = "Warning: User has no supervisor access to any of the tasks"

                    MessageBox.Show("Warning: User has no supervisor access to any of the tasks", "iPMWrkTaskInstance", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Else
                    barStatus.Items.Item(0).Text = "Warning: There are no user groups that this collection of tasks can be assigned to"

                    MessageBox.Show("Warning: There are no user groups that this collection of tasks can be assigned to", "iPMWrkTaskInstance", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                End If

                Return result
            End If

            ' load the user group combo with the list of available user groups

            For iCounter As Integer = vUserGroups.GetLowerBound(1) To vUserGroups.GetUpperBound(1)

                cboUserGroup.Items.Add(CStr(vUserGroups(1, iCounter)))

                VB6.SetItemData(cboUserGroup, cboUserGroup.Items.Count - 1, CInt(vUserGroups(0, iCounter)))
            Next

            ' ensure that no user group is shown initially
            cboUserGroup.SelectedIndex = -1

            m_bDirty = False
            m_bFormLoaded = True

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function


    ' ***************************************************************** '
    ' Name: AddScheduledTaskToList
    '
    ' Description: Builds the Scheduled Task List View from the Array
    '              Supplied.
    ' AMB 21/01/2003 - Added workflow_information field - IAG Spec 229'
    ' ***************************************************************** '
    Public Function AddScheduledTaskToList(ByVal v_sKey As String, ByVal v_iIsUrgent As Integer, ByVal v_sTaskStatusDesc As String, ByVal v_sTaskTypedesc As String, ByVal v_dtTaskDueDate As Date, ByVal v_sCustomer As String, ByVal v_sDescription As String, ByVal v_sUserGroup As String, ByVal v_sUser As String, ByVal v_sWorkflowInformation As String) As Integer
        ' AMB 21/01/2003

        Dim result As Integer = 0
        'Const ACSTUrgentCol As Integer = 0         ''Unused Local Variable
        Const ACSTStatusCol As Integer = 1
        Const ACSTDueDateCol As Integer = 2
        Const ACSTDescriptionCol As Integer = 3
        Const ACSTCustomerCol As Integer = 4
        Const ACSTUserCol As Integer = 5
        ' AMB 21/01/2003
        Const ACSTWorkflowInformation As Integer = 6

        Dim oListItem As ListViewItem

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If v_iIsUrgent = gPMConstants.PMEReturnCode.PMTrue Then
                ' Set oListItem = lstScheduledTasks.ListItems.Add(, v_sKey, "Yes", , "Urgent")
                oListItem = lstScheduledTasks.Items.Add(v_sKey, "Yes", "")
                ' AMB 21/01/2003 - removed reference to icon - there is no imagelist on this form!
            Else
                oListItem = lstScheduledTasks.Items.Add(v_sKey, "No", "")
            End If

            ListViewHelper.GetListViewSubItem(oListItem, ACSTStatusCol).Text = v_sTaskStatusDesc.Trim()
            ListViewHelper.GetListViewSubItem(oListItem, ACSTDueDateCol).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatDateShort, vFieldValue:=DateTimeHelper.ToString(v_dtTaskDueDate))
            ListViewHelper.GetListViewSubItem(oListItem, ACSTDescriptionCol).Text = v_sDescription.Trim()
            ListViewHelper.GetListViewSubItem(oListItem, ACSTCustomerCol).Text = v_sCustomer.Trim()
            ListViewHelper.GetListViewSubItem(oListItem, ACSTUserCol).Text = v_sUser.Trim()
            ' AMB 21/01/2003
            ListViewHelper.GetListViewSubItem(oListItem, ACSTWorkflowInformation).Text = v_sWorkflowInformation.Trim()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddScheduledTaskToListFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddScheduledTaskToList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: RefreshUserGroupList
    '
    ' Description: Shows the User Groups in the Selected Task Group
    '
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (RefreshUserGroupList) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub RefreshUserGroupList(Optional ByVal v_iTaskGroupId As Integer = 0)
    '
    'Try 
    '
    'cboTaskUserGroup.FirstItem = "Any Task Group"
    'cboTaskUserGroup.PMTaskGroupID = cboWrkTaskGroup.ItemId
    '
    'If v_iTaskGroupId > 0 Then
    '        cboTaskUserGroup.DefaultTaskGroupID = v_iTaskGroupId
    'End If
    '
    '    cboTaskUserGroup.RefreshList
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Log Error Message
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RefreshUserGroupList Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RefreshUserGroupList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Exit Sub
    '
    'End Try
    '
    'End Sub

    ' ***************************************************************** '
    ' Name: RefreshUserList
    '
    ' Description: Shows the Users in the Selected User Group
    '
    '
    ' ***************************************************************** '
    Private Sub RefreshUserList(Optional ByVal v_iUserId As Integer = 0)

        Try

            '    cboTaskUser.FirstItem = "Any Group Member"
            '    cboTaskUser.PMUserGroupID = cboTaskUserGroup.UserGroupID
            If v_iUserId > 0 Then
                '        cboTaskUser.DefaultUserID = v_iUserId
            End If
            '    cboTaskUser.RefreshList

        Catch excep As System.Exception




            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RefreshUserListFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="RefreshUserList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

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
        Const ACTaskStatusDescNew As String = "New"
        Const ACTaskStatusDescInProgress As String = "In Progress"
        Const ACTaskStatusDescInComplete As String = "InComplete"
        Const ACTaskStatusDescComplete As String = "Complete"

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

    'UPGRADE_NOTE: (7001) The following declaration (cboTaskUser_Click) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub cboTaskUser_Click()
    'm_bDirty = True
    'End Sub

    'UPGRADE_NOTE: (7001) The following declaration (cboTaskUserGroup_Click) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub cboTaskUserGroup_Click()
    ' Show the Users in the Selected Group
    '
    'RefreshUserList()
    '
    'm_bDirty = True
    '
    'End Sub

    Private Sub cboUserGroup_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboUserGroup.SelectedIndexChanged


        Dim iIndex As Integer = cboUserGroup.SelectedIndex

        If iIndex >= 0 Then
            m_lReturn = CType(GetGroupUsers(VB6.GetItemData(cboUserGroup, iIndex)), gPMConstants.PMEReturnCode)
        End If

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        ' Click event of the OK button.

        Try

            ' Set the interface status.

            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = CType(ProcessCommand(), gPMConstants.PMEReturnCode)

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        ' Click event of the Cancel button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = CType(ProcessCommand(), gPMConstants.PMEReturnCode)

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: ProcessCommand
    '
    ' Description: Determines which action to take on the details
    '              depending upon the task and interface state.
    '
    ' ***************************************************************** '
    Private Function ProcessCommand() As Integer

        Dim result As Integer = 0
        Dim iMsgResult As DialogResult
        Dim sMessage, sTitle As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check the task.
            Select Case (m_iTask)
                Case gPMConstants.PMEComponentAction.PMAdd, gPMConstants.PMEComponentAction.PMEdit, gPMConstants.PMEComponentAction.PMView

                Case Else
                    ' Check if form has been cancelled, if so,
                    ' check if the details have changed and if
                    ' so, prompt if they wish to cancel.
                    If m_lStatus = gPMConstants.PMEReturnCode.PMCancel Then

                        If m_bDirty Then
                            sTitle = ACCancelDetailsTitleText
                            sMessage = ACCancelDetailsText

                            iMsgResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

                            ' Check message result.
                            If iMsgResult = System.Windows.Forms.DialogResult.No Then
                                ' Set return to false, meaning
                                ' don't cancel.
                                result = gPMConstants.PMEReturnCode.PMFalse
                            End If
                        End If
                    Else

                        If cboUserGroup.SelectedIndex = -1 Then
                            result = gPMConstants.PMEReturnCode.PMFalse

                            MessageBox.Show("Please select a user group (and optionally, a user)", "iPMWrkTaskInstance", MessageBoxButtons.OK, MessageBoxIcon.Information)

                            Return result
                        End If

                        ' Update the details using the business object.
                        m_lReturn = CType(InterfaceToBusiness(), gPMConstants.PMEReturnCode)

                        ' Check for errors.
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' Failed to update the details
                            result = gPMConstants.PMEReturnCode.PMFalse

                            ' Log Error.
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to update the details", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand")
                        End If
                    End If

            End Select

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process command", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: InterfaceToBusiness
    '
    ' Description: Updates all business members from the interface
    '              details.
    '
    ' ***************************************************************** '
    Private Function InterfaceToBusiness() As Integer

        Dim result As Integer = 0
        Dim lUserID, lUserGroupID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lUserID = VB6.GetItemData(cboGroupUsers, cboGroupUsers.SelectedIndex)
            lUserGroupID = VB6.GetItemData(cboUserGroup, cboUserGroup.SelectedIndex)


            m_lReturn = m_oBusiness.ReAssignMultipleTask(m_vPMWrkTaskInstanceCntArray, lUserGroupID, lUserID)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to assign the interface details to business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness")
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetGroupUsers
    '
    ' Author: RDC 06092002
    ' Description:
    '
    ' ***************************************************************** '
    Private Function GetGroupUsers(ByVal lUserGroupID As Integer) As Integer

        Dim result As Integer = 0
        Dim vGroupUsers As Object

        Try

            result = gPMConstants.PMEReturnCode.PMFalse


            m_lErrorNumber = m_oBusiness.GetGroupUsers(lUserGroupID, vGroupUsers)

            If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to get group users", "iPMWrkTaskInstance", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return result
            End If

            cboGroupUsers.Items.Clear()

            cboGroupUsers.Items.Add("(Any user)")
            VB6.SetItemData(cboGroupUsers, 0, -1)


            For lLoop As Integer = vGroupUsers.GetLowerBound(1) To vGroupUsers.GetUpperBound(1)

                cboGroupUsers.Items.Add(CStr(vGroupUsers(GROUP_USER_NAME, lLoop)).Trim())

                VB6.SetItemData(cboGroupUsers, cboGroupUsers.Items.Count - 1, CInt(vGroupUsers(GROUP_USER_ID, lLoop)))
            Next

            cboGroupUsers.SelectedIndex = 0


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch



            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function

    ' ***************************************************************** '
    ' Name: CheckIsSupervisor
    '
    ' Author: RDC 06092002
    ' Description: check if user is a supervisor of the supplied
    '              user group
    '
    ' ***************************************************************** '
    Private Function CheckIsSupervisor(ByVal iUserID As Integer, ByVal lPMUserGroupID As Integer, ByRef iIsSupervisor As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse


            m_lReturn = m_oBusiness.CheckIsSupervisor(iUserID, lPMUserGroupID, iIsSupervisor)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch



            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function

    ' ***************************************************************** '
    ' Name: GetTaskUserGroups
    '
    ' Author: RDC 09092002
    ' Description: get user groups that can execute the supplied task
    '
    ' ***************************************************************** '
    Private Function GetTaskUserGroups(ByVal lTaskID As Integer, ByRef vUserGroups(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse


            m_lReturn = m_oBusiness.GetTaskUserGroups(lTaskID, vUserGroups)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch



            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function

    Private Sub frmAssignMultipleTask_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        'Developer Guide No 293
        If e.Alt And e.KeyCode = Keys.D1 Then
            tabMainTab.SelectedIndex = 0
        End If
    End Sub
End Class
