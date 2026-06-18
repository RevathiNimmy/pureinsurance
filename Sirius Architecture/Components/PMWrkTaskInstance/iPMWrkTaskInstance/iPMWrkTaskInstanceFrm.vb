Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Drawing
Imports System.Windows.Forms
'Developer Guide No.129
Imports SharedFiles
Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form

    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date: 20/01/1998
    '
    ' Description: Main interface for Task form
    '
    ' Edit History:
    ' DAK070999 - Allow date input in txtDueDate to be recorded
    ' DAK131099 - changes for licencing and linked objects
    ' AMB 21/01/2003 - Added workflow_information field - IAG Spec 229
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmInterface"
    'Developer Guide No. 7
    Private Const vbFormCode As Integer = 0
    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' {* GENERATED CODE (Begin) *}
    Private m_lPMWrkTaskInstanceCnt As Integer
    Private m_lPMWrkTaskGroupId As Integer
    Private m_lPMWrkTaskId As Integer
    Private m_sCustomer As String = ""
    Private m_dtTaskDueDate As Date
    Private m_lPmuserGroupID As Integer
    Private m_iUserID As Integer
    Private m_sDescription As String = ""
    Private m_iTaskStatus As gPMConstants.PMEWrkManTaskStatus
    Private m_iIsUrgent As CheckState
    Private m_dtDateCreated As Date
    Private m_iCreatedByID As Integer
    Private m_dtLastModified As Date
    Private m_iModifiedByID As Integer
    ' AMB 20/01/2003 - Add workflow information property
    Private m_sWorkflowInformation As String = ""

    Private m_iIsTaskReview As CheckState
    ' {* GENERATED CODE (End) *}

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As gPMConstants.PMEReturnCode
    Private m_lErrorNumber As Integer

    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' {* USER DEFINED CODE (Begin) *}
    Private m_lListIndex As Integer

    ' Declare an instance of the Form Control object.
    Private m_oFormFields As iPMFormControl.FormFields

    Private m_lTaskGroup As Integer

    Private m_iIsSystemTask As Integer
    Private m_iTypeOfTask As Integer
    Private m_lPMNavProcessID As Integer
    Private m_sComponentObjectName As String = ""
    Private m_sComponentClassName As String = ""
    Private m_lAutoDeleteAfterNumDays As Integer
    'DAK131099
    ' DisplayIcon
    Private m_lDisplayIcon As Integer
    ' IsViewOnlyTask
    Private m_iIsViewOnlyTask As Integer
    ' LinkedObjectName
    Private m_sLinkedObjectName As String = ""
    ' LinkedClassName
    Private m_sLinkedClassName As String = ""
    ' LinkedCaption
    Private m_sLinkedCaption As String = ""
    ' IsAvailableTask
    Private m_iIsAvailableTask As Integer
    ' PMAuthorityLevel
    Private m_lPMAuthorityLevel As Integer
    ' LinkedKeysAdded
    Private m_bLinkedKeysAdded As Boolean

    Private m_vTaskInstKeyArray As Object

    Private m_bFormLoaded As Boolean

    Private m_bDirty As Boolean

    ' {* USER DEFINED CODE (End) *}

    ' Declare an instance of the Business object.
    'Private m_oBusiness As bPMWrkTaskInstance.FormClass

    Private m_oBusiness As bPMWrkTaskInstance.FormClass

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer

    Private m_bDisableCustomer As Boolean

    Private m_sNavXMLfile As String = ""
    'WPR 33-75 added
    Private m_iSourceId As Integer 'wpr27
    Private m_iUserPartyKey As Integer

    Public Property IsTaskReview() As Integer
        Get

            Return m_iIsTaskReview

        End Get
        Set(ByVal Value As Integer)

            m_iIsTaskReview = Value

        End Set
    End Property

    Public Property NavXMLfile() As String
        Get
            Return m_sNavXMLfile
        End Get
        Set(ByVal Value As String)
            m_sNavXMLfile = Value
        End Set
    End Property


    Public Property DisableCustomer() As Boolean
        Get
            Return m_bDisableCustomer
        End Get
        Set(ByVal Value As Boolean)
            m_bDisableCustomer = Value
        End Set
    End Property

    ' Stores the details from the business object.

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

    Public Property Status() As Integer
        Get

            ' Standard Property.

            ' Return the interface exit status.
            Return m_lStatus

        End Get
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the interface exit status.
            m_lStatus = Value

        End Set
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

    ' {* USER DEFINED CODE (Begin) *}

    Public Property PMWrkTaskInstanceCnt() As Integer
        Get

            Return m_lPMWrkTaskInstanceCnt

        End Get
        Set(ByVal Value As Integer)

            m_lPMWrkTaskInstanceCnt = Value

        End Set
    End Property


    Public Property PMWrkTaskGroupId() As Integer
        Get

            Return m_lPMWrkTaskGroupId

        End Get
        Set(ByVal Value As Integer)

            m_lPMWrkTaskGroupId = Value

        End Set
    End Property


    Public Property PMWrkTaskId() As Integer
        Get

            Return m_lPMWrkTaskId

        End Get
        Set(ByVal Value As Integer)

            m_lPMWrkTaskId = Value

        End Set
    End Property


    Public Property Customer() As String
        Get

            Return m_sCustomer

        End Get
        Set(ByVal Value As String)

            m_sCustomer = Value

        End Set
    End Property


    Public Property DueDate() As Date
        Get

            Return m_dtTaskDueDate

        End Get
        Set(ByVal Value As Date)

            m_dtTaskDueDate = Value

        End Set
    End Property


    Public Property PMUserGroupID() As Integer
        Get

            Return m_lPmuserGroupID

        End Get
        Set(ByVal Value As Integer)

            m_lPmuserGroupID = Value

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


    Public Property Description() As String
        Get

            Return m_sDescription

        End Get
        Set(ByVal Value As String)

            m_sDescription = Value

        End Set
    End Property

    Public Property WorkflowInformation() As String
        Get
            ' AMB 20/01/2003 - Add workflow information property

            Return m_sWorkflowInformation

        End Get
        Set(ByVal Value As String)
            ' AMB 20/01/2003 - Add workflow information property

            m_sWorkflowInformation = Value

        End Set
    End Property

    Public Property TaskStatus() As Integer
        Get

            Return m_iTaskStatus

        End Get
        Set(ByVal Value As Integer)

            m_iTaskStatus = Value

        End Set
    End Property
    Public Property TaskCreatedDate() As Date
        Get
            ' Get the task Created Date
            Return m_dtDateCreated
        End Get
        Set(ByVal Value As Date)
            ' Set the task Created Date
            m_dtDateCreated = Value
        End Set
    End Property


    Public Property TaskCreatedByID() As Integer
        Get
            ' Get the task Created By ID
            Return m_iCreatedByID
        End Get
        Set(ByVal Value As Integer)
            ' Set the task Created Date
            m_iCreatedByID = Value
        End Set
    End Property

    Public Property IsUrgent() As Integer
        Get

            Return m_iIsUrgent

        End Get
        Set(ByVal Value As Integer)

            m_iIsUrgent = Value

        End Set
    End Property


    Public Property IsSystemTask() As Integer
        Get

            Return m_iIsSystemTask

        End Get
        Set(ByVal Value As Integer)

            m_iIsSystemTask = Value

        End Set
    End Property


    Public Property TypeOfTask() As Integer
        Get

            Return m_iTypeOfTask

        End Get
        Set(ByVal Value As Integer)

            m_iTypeOfTask = Value

        End Set
    End Property

    Public Property PMNavProcessId() As Integer
        Get
            Return m_lPMNavProcessID
        End Get
        Set(ByVal Value As Integer)
            m_lPMNavProcessID = Value
        End Set
    End Property

    Public Property ComponentObjectName() As String
        Get
            Return m_sComponentObjectName.Trim()
        End Get
        Set(ByVal Value As String)
            m_sComponentObjectName = Value.Trim()
        End Set
    End Property

    Public Property ComponentClassName() As String
        Get
            Return m_sComponentClassName.Trim()
        End Get
        Set(ByVal Value As String)
            m_sComponentClassName = Value.Trim()
        End Set
    End Property

    Public Property AutoDeleteAfterNumDays() As Integer
        Get
            Return m_lAutoDeleteAfterNumDays
        End Get
        Set(ByVal Value As Integer)
            m_lAutoDeleteAfterNumDays = Value
        End Set
    End Property

    'DAK131099
    Public Property DisplayIcon() As Integer
        Get
            Return m_lDisplayIcon
        End Get
        Set(ByVal Value As Integer)
            m_lDisplayIcon = Value
        End Set
    End Property

    Public Property IsViewOnlyTask() As Integer
        Get
            Return m_iIsViewOnlyTask
        End Get
        Set(ByVal Value As Integer)
            m_iIsViewOnlyTask = Value
        End Set
    End Property

    Public Property LinkedObjectName() As String
        Get
            Return m_sLinkedObjectName.Trim()
        End Get
        Set(ByVal Value As String)
            m_sLinkedObjectName = Value.Trim()
        End Set
    End Property

    Public Property LinkedClassName() As String
        Get
            Return m_sLinkedClassName.Trim()
        End Get
        Set(ByVal Value As String)
            m_sLinkedClassName = Value.Trim()
        End Set
    End Property

    Public Property LinkedCaption() As String
        Get
            Return m_sLinkedCaption.Trim()
        End Get
        Set(ByVal Value As String)
            m_sLinkedCaption = Value.Trim()
        End Set
    End Property

    Public Property IsAvailableTask() As Integer
        Get
            Return m_iIsAvailableTask
        End Get
        Set(ByVal Value As Integer)
            m_iIsAvailableTask = Value
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

    Public WriteOnly Property TaskInstKeyArray() As Object
        Set(ByVal Value As Object)
            If Information.IsArray(Value) Then


                m_vTaskInstKeyArray = Value
            Else

                m_vTaskInstKeyArray = ""
            End If
        End Set
    End Property

    Public Property LinkedKeysAdded() As Boolean
        Get
            Return m_bLinkedKeysAdded
        End Get
        Set(ByVal Value As Boolean)
            m_bLinkedKeysAdded = Value
        End Set
    End Property
    'WPR 33-75 added
    Public Property SourceId() As Integer
        Get
            Return m_iSourceId
        End Get
        Set(ByVal Value As Integer)
            m_iSourceId = Value
        End Set
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

            m_lReturn = Load_Renamed()
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
            m_lReturn = SetInterfaceDefaults()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return result
            End If

            'The shifted code of 'frmInterface_Load'.....
            m_lReturn = SetFieldValidation()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Status = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Function
            End If
            'developers giude no 220
            Me.cboWrkTaskGroup.FirstItem = ""
            Me.cboTaskUserGroup.FirstItem = ""
            Me.cboTaskUser.FirstItem = ""
            Me.cboWrkTask.FirstItem = ""
            Me.cboLoggedByUser.FirstItem = ""
            Me.cboModifiedByUser.FirstItem = ""

            ' Gets the interface details to be displayed.
            m_lReturn = GetInterfaceDetails()

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
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the interface details.

            m_bFormLoaded = False

            ' Assign the details from the business object
            ' to the data storage.
            m_lReturn = BusinessToData()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Assign the details to the interface.

            cboLoggedByUser.SingleUserID = m_iCreatedByID
            cboLoggedByUser.RefreshList()
            txtLoggedByUser.Text = cboLoggedByUser.ItemUsername

            m_lReturn = m_oFormFields.FormatControl(txtLoggedDate, m_dtDateCreated)
            m_lReturn = m_oFormFields.FormatControl(txtLoggedTime, m_dtDateCreated)

            If m_iModifiedByID > 0 Then

                cboModifiedByUser.SingleUserID = m_iModifiedByID
                cboModifiedByUser.RefreshList()
                txtModifiedByUser.Text = cboModifiedByUser.ItemUsername

                m_lReturn = m_oFormFields.FormatControl(txtModifiedDate, m_dtLastModified)
                m_lReturn = m_oFormFields.FormatControl(txtModifiedTime, m_dtLastModified)

            Else
                txtModifiedDate.Text = ""
                txtModifiedTime.Text = ""
            End If

            If m_lPMWrkTaskGroupId <> 0 Then
                cboWrkTaskGroup.ItemId = m_lPMWrkTaskGroupId
            End If

            If cboWrkTaskGroup.ListIndex = 0 Then
                cboWrkTaskGroup_Click(cboWrkTaskGroup, Nothing)
            End If

            If m_lPMWrkTaskId <> 0 Then
                cboWrkTask.PMTaskGroupID = m_lPMWrkTaskId
            End If

            '    If cboWrkTask.ListIndex = 0 Then
            '        Call cboWrkTask_Click
            '    End If

            m_lReturn = m_oFormFields.FormatControl(txtDueDate, m_dtTaskDueDate)
            m_lReturn = m_oFormFields.FormatControl(txtDueTime, m_dtTaskDueDate)

            m_lReturn = m_oFormFields.FormatControl(txtCustomer, m_sCustomer)
            m_lReturn = m_oFormFields.FormatControl(txtDescription, m_sDescription)
            ' AMB 21/01/2003
            m_lReturn = m_oFormFields.FormatControl(txtWorkflowInfo, m_sWorkflowInformation)

            chkIsUrgent.CheckState = m_iIsUrgent

            If m_iTaskStatus = gPMConstants.PMEWrkManTaskStatus.pmeWMTSComplete Then
                chkIsComplete.CheckState = CheckState.Checked
            Else
                chkIsComplete.CheckState = CheckState.Unchecked
            End If

            If m_lPmuserGroupID > 0 Then
                cboTaskUserGroup.UserGroupID = m_lPmuserGroupID
            End If

            chkIsTaskReview.CheckState = m_iIsTaskReview
            If m_iTask <> gPMConstants.PMEComponentAction.PMAdd Then
                chkIsTaskReview.Enabled = False
            End If
            '    If (m_lPMWrkTaskGroupId > 0) Then
            '        RefreshUserGroupList v_iTaskGroupId:=m_lPMWrkTaskGroupId
            '    Else
            '        RefreshUserGroupList
            '    End If
            '
            '    If (m_lPMWrkTaskId > 0) Then
            '        RefreshTaskList v_iTaskId:=m_lPMWrkTaskId
            '    Else
            '        RefreshTaskList
            '    End If

            RefreshUserList(v_iUserId:=m_iUserID)

            'DAK131099
            If LinkedCaption = "" Then
                cmdLinkObject.Visible = False
            Else
                cmdLinkObject.Text = LinkedCaption
                cmdLinkObject.Visible = True
                'DAK080800 - set width of link button
                cmdLinkObject.Width = CreateGraphics().MeasureString(cmdLinkObject.Text, Font).Width + VB6.TwipsToPixelsX(150)
                If VB6.PixelsToTwipsX(cmdLinkObject.Width) > VB6.PixelsToTwipsX(cmdTaskLog.Left) - VB6.PixelsToTwipsX(cmdLinkObject.Left) Then
                    cmdLinkObject.Width = cmdTaskLog.Left - cmdLinkObject.Left
                End If
            End If


            imgIcon2.Image = imgIcon1.Image

            m_bDirty = (m_iTask = gPMConstants.PMEComponentAction.PMAdd)

            If gPMFunctions.ToSafeDate(txtDueDate.Text) = DateTime.Today Then
                For iIndex As Integer = 0 To cboDueDate.Items.Count - 1
                    If gPMFunctions.ToSafeLong(VB6.GetItemData(cboDueDate, iIndex)) = 1 Then
                        cboDueDate.SelectedIndex = iIndex
                        Exit For
                    End If
                Next iIndex
            End If

            If gPMFunctions.ToSafeDate(txtDueDate.Text) = gPMFunctions.ToSafeDate(DateAdd(DateInterval.Weekday, 1, ToSafeDate(txtLoggedDate.Text))) Then
                For nIndex As Integer = 0 To cboDueDate.Items.Count - 1
                    If gPMFunctions.ToSafeLong(VB6.GetItemData(cboDueDate, nIndex)) = 7 Then
                        m_lListIndex = nIndex
                        cboDueDate.SelectedIndex = nIndex
                        Exit For
                    End If
                Next nIndex
            End If
            m_bFormLoaded = True

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: InterfaceToBusiness
    '
    ' Description: Updates all business members from the interface
    '              details.
    '
    ' AMB 21/01/2003 - Added workflow_information field - IAG Spec 229
    ' ***************************************************************** '
    Private Function InterfaceToBusiness() As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the business object.

            ' Assign the details from the interface to the data storage.
            m_lReturn = InterfaceToData()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' Check the task.

            Select Case (Task)
                ' Add a Task Instance
                Case gPMConstants.PMEComponentAction.PMAdd

                    ' If we have been supplied with some Key Values to
                    ' add with the Task, the pass them.
                    If Information.IsArray(m_vTaskInstKeyArray) Then

                        m_lReturn = m_oBusiness.CreateNew(r_lpmwrktaskinstancecnt:=m_lPMWrkTaskInstanceCnt, v_lPMWrkTaskGroupID:=m_lPMWrkTaskGroupId, v_lPMWrkTaskID:=m_lPMWrkTaskId, v_sCustomer:=m_sCustomer, v_dtTaskDueDate:=m_dtTaskDueDate, v_lpmusergroupid:=m_lPmuserGroupID, v_iUserId:=m_iUserID, v_sDescription:=m_sDescription, v_itaskstatus:=m_iTaskStatus, v_iIsUrgent:=m_iIsUrgent, v_dtdatecreated:=m_dtDateCreated, v_icreatedbyid:=m_iCreatedByID, v_vKeyArray:=m_vTaskInstKeyArray, v_sWorkflowInformation:=m_sWorkflowInformation, v_iIsTaskReview:=m_iIsTaskReview)
                        ' AMB 21/01/2003
                    Else

                        m_lReturn = m_oBusiness.CreateNew(r_lpmwrktaskinstancecnt:=m_lPMWrkTaskInstanceCnt, v_lPMWrkTaskGroupID:=m_lPMWrkTaskGroupId, v_lPMWrkTaskID:=m_lPMWrkTaskId, v_sCustomer:=m_sCustomer, v_dtTaskDueDate:=m_dtTaskDueDate, v_lpmusergroupid:=m_lPmuserGroupID, v_iUserId:=m_iUserID, v_sDescription:=m_sDescription, v_itaskstatus:=m_iTaskStatus, v_iIsUrgent:=m_iIsUrgent, v_dtdatecreated:=m_dtDateCreated, v_icreatedbyid:=m_iCreatedByID, v_sWorkflowInformation:=m_sWorkflowInformation, v_iIsTaskReview:=m_iIsTaskReview)
                        ' AMB 21/01/2003
                    End If

                    ' Amend Details
                Case gPMConstants.PMEComponentAction.PMEdit

                    m_lReturn = m_oBusiness.AmendDetails(v_lPMWrkTaskInstanceCnt:=m_lPMWrkTaskInstanceCnt, v_sCustomer:=m_sCustomer, v_dtTaskDueDate:=m_dtTaskDueDate, v_sDescription:=m_sDescription, v_iIsUrgent:=m_iIsUrgent, v_dtlastmodified:=m_dtLastModified, v_imodifiedbyid:=m_iModifiedByID, v_sWorkflowInformation:=m_sWorkflowInformation, v_iIsTaskReview:=m_iIsTaskReview)
                    ' AMB 21/01/2003

                    ' View
                Case gPMConstants.PMEComponentAction.PMView
                    ' Nothing to do here

                    ' ReAssign
                Case Else

                    m_lReturn = m_oBusiness.ReAssign(v_lPMWrkTaskInstanceCnt:=m_lPMWrkTaskInstanceCnt, v_lpmusergroupid:=m_lPmuserGroupID, v_iUserId:=m_iUserID)

            End Select

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

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DisplayLookupDetails
    '
    ' Description: Displays all of the lookup details using the lookup
    '              values/details.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (DisplayLookupDetails) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function DisplayLookupDetails() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Get the lookup values.
    '
    'm_lReturn = GetLookupValues()
    '
    ' Check for errors.
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    ' Get all of the lookup details.
    '
    ' {* USER DEFINED CODE (Begin) *}
    '
    ' ************************************************************
    ' Enter your code here to retrieve all of the lookup
    ' descriptions for a given lookup type.
    ' The GetLookupDetails function will allow you to do this.
    ''
    ' Example:-
    ''
    '    m_lReturn& = GetLookupDetails( _
    ''        sLookupTable:=PMLookupCodeName, _
    ''        ctlLookup:=cmbCodeName)
    ''
    '    ' Check for errors.
    '    If (m_lReturn& <> PMTrue) Then
    '        DisplayLookupDetails = PMFalse
    '        Exit Function
    '    End If
    ''
    ' NOTE: Replace this section with your new code.
    ' ************************************************************
    '
    ' {* USER DEFINED CODE (End) *}
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error Section
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: SetFieldValidation
    '
    ' Description: Sets the rules for validating fields.
    '
    ' AMB 21/01/2003 - Added workflow_information field - IAG Spec 229
    ' ***************************************************************** '
    Private Function SetFieldValidation() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oFormFields = New iPMFormControl.FormFields()

            m_oFormFields.LanguageID = g_iLanguageID

            ' {* GENERATED CODE (Begin) *}
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtDueDate, lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtDueTime, lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatTimeLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtCustomer, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtDescription, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtLoggedByUser, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtLoggedDate, lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtLoggedTime, lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatTimeLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtModifiedByUser, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtModifiedDate, lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtModifiedTime, lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatTimeLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' AMB 21/01/2003
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtWorkflowInfo, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' {* GENERATED CODE (End) *}

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetFieldValidation", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFieldValidation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: BusinessToData
    '
    ' Description: Updates the data storage from the business object.
    '
    ' AMB 20/01/2003 - Add workflow information property
    ' ***************************************************************** '
    Private Function BusinessToData() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the details to the data storage.

            ' {* USER DEFINED CODE (Begin) *}

            If Task = gPMConstants.PMEComponentAction.PMAdd Then
                m_dtDateCreated = DateTime.Now
                m_iCreatedByID = g_oObjectManager.UserID
                If m_dtTaskDueDate < #12/31/1899# Then
                    m_dtTaskDueDate = m_dtDateCreated
                End If
            Else

                m_lReturn = m_oBusiness.GetDetails(v_lPMWrkTaskInstanceCnt:=m_lPMWrkTaskInstanceCnt, r_lPMWrkTaskGroupID:=m_lPMWrkTaskGroupId, r_lpmwrktaskid:=m_lPMWrkTaskId, r_scustomer:=m_sCustomer, r_dttaskduedate:=m_dtTaskDueDate, r_lpmusergroupid:=m_lPmuserGroupID, r_iuserid:=m_iUserID, r_sdescription:=m_sDescription, r_itaskstatus:=m_iTaskStatus, r_iisurgent:=m_iIsUrgent, r_dtdatecreated:=m_dtDateCreated, r_icreatedbyid:=m_iCreatedByID, r_dtlastmodified:=m_dtLastModified, r_imodifiedbyid:=m_iModifiedByID, r_sWorkflowInformation:=m_sWorkflowInformation, r_iIsTaskReview:=m_iIsTaskReview)
                ' AMB 20/01/2003

                ' {* USER DEFINED CODE (End) *}

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
                End If
            End If

            m_lTaskGroup = cboWrkTaskGroup.ItemId


            m_lReturn = m_oBusiness.GetTaskDetails(v_lPMWrkTaskID:=m_lPMWrkTaskId, r_iIsSystemTask:=m_iIsSystemTask, r_iTypeOfTask:=m_iTypeOfTask, r_lPMNavProcessID:=m_lPMNavProcessID, r_sComponentObjectName:=m_sComponentObjectName, r_sComponentClassName:=m_sComponentClassName, r_lAutoDeleteAfterNumDays:=m_lAutoDeleteAfterNumDays, r_lDisplayIcon:=m_lDisplayIcon, r_iIsViewOnlyTask:=m_iIsViewOnlyTask, r_sLinkedObjectName:=m_sLinkedObjectName, r_sLinkedClassName:=m_sLinkedClassName, r_sLinkedCaption:=m_sLinkedCaption, r_iIsAvailableTask:=m_iIsAvailableTask, r_sNavXMLFile:=m_sNavXMLfile)

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: InterfaceToData
    '
    ' Description: Updates the data storage from the interface details.
    '
    ' AMB 21/01/2003 - Added workflow_information field - IAG Spec 229
    ' ***************************************************************** '
    Private Function InterfaceToData() As Integer
        Dim result As Integer = 0
        Dim dtDueDate As Date
        Dim dtDueTime As DateTime

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' If we have modified this Task
            If (Task = gPMConstants.PMEComponentAction.PMAdd) Or (Task = gPMConstants.PMEComponentAction.PMView) Then
            Else
                ' Update the last Modified Details
                m_dtLastModified = DateTime.Now
                m_iModifiedByID = g_oObjectManager.UserID
            End If

            m_lPMWrkTaskGroupId = cboWrkTaskGroup.ItemId
            m_lPMWrkTaskId = cboWrkTask.TaskID


            dtDueDate = CDate(m_oFormFields.UnformatControl(txtDueDate))
            'developer guide no. 300
            If txtDueTime.Text.Trim = "" Then
                m_dtTaskDueDate = dtDueDate.AddDays(-1)
            Else
                dtDueTime = txtDueTime.Text.Trim
                m_dtTaskDueDate = dtDueDate.AddDays(dtDueTime.ToOADate())
            End If

            m_sCustomer = CStr(m_oFormFields.UnformatControl(txtCustomer))

            m_sDescription = CStr(m_oFormFields.UnformatControl(txtDescription))
            ' AMB 21/01/2003

            m_sWorkflowInformation = CStr(m_oFormFields.UnformatControl(txtWorkflowInfo))

            m_iIsUrgent = chkIsUrgent.CheckState

            If chkIsComplete.CheckState = CheckState.Checked Then
                m_iTaskStatus = gPMConstants.PMEWrkManTaskStatus.pmeWMTSComplete
            End If

            m_lPmuserGroupID = cboTaskUserGroup.UserGroupID
            m_iUserID = cboTaskUser.UserID
            m_iIsTaskReview = chkIsTaskReview.CheckState


            m_lReturn = m_oBusiness.GetTaskDetails(v_lPMWrkTaskID:=m_lPMWrkTaskId, r_iIsSystemTask:=m_iIsSystemTask, r_iTypeOfTask:=m_iTypeOfTask, r_lPMNavProcessID:=m_lPMNavProcessID, r_sComponentObjectName:=m_sComponentObjectName, r_sComponentClassName:=m_sComponentClassName, r_lAutoDeleteAfterNumDays:=m_lAutoDeleteAfterNumDays, r_lDisplayIcon:=m_lDisplayIcon, r_iIsViewOnlyTask:=m_iIsViewOnlyTask, r_sLinkedObjectName:=m_sLinkedObjectName, r_sLinkedClassName:=m_sLinkedClassName, r_sLinkedCaption:=m_sLinkedCaption, r_iIsAvailableTask:=m_iIsAvailableTask, r_sNavXMLfile:=m_sNavXMLfile)

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        Dim vDiaryDays As Object
        Dim vArray(,) As Object
        Dim iUbound As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.
            iPMFunc.CenterForm(Me)

            ' Display all language specific captions.
            m_lReturn = DisplayCaptions()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the status of the Navigate button.
            Select Case (m_lNavigate)
                Case gPMConstants.PMENavigateButtonStatus.PMNavigateEnabled
                    cmdNavigate.Visible = True
                    cmdNavigate.Enabled = True

                Case gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled
                    cmdNavigate.Visible = True
                    cmdNavigate.Enabled = False

                Case Else
                    cmdNavigate.Visible = False
            End Select

            ' Set any other default values to the interface.

            ' These Controls are always disabled
            txtLoggedByUser.Enabled = False
            txtLoggedDate.Enabled = False
            txtLoggedTime.Enabled = False
            txtModifiedByUser.Enabled = False
            txtModifiedDate.Enabled = False
            txtModifiedTime.Enabled = False

            'DAK120700
            cboDueDate.Items.Clear()
            'set up the allowed values in the due date combo box
            'DAK070999
            'PN 27272 14/03/06
            ReDim vArray(1, 5)


            vArray(0, 0) = "Today"

            vArray(1, 0) = 1

            vArray(0, 1) = "Tomorrow"

            vArray(1, 1) = 2

            vArray(0, 2) = "Within a Week"

            vArray(1, 2) = 7

            vArray(0, 3) = "Within a Month"

            vArray(1, 3) = 31

            vArray(0, 4) = "Within a Quarter"

            vArray(1, 4) = 90

            vArray(0, 5) = "Within a Year"

            vArray(1, 5) = 365

            '2005 user Definable Diary Days

            m_lReturn = m_oBusiness.GetDiaryDays(vDiaryDays)
            If Information.IsArray(vDiaryDays) Then

                iUbound = vDiaryDays.GetUpperBound(1) + 1
                ReDim Preserve vArray(1, iUbound + vArray.GetUpperBound(1))


                For lCount As Integer = 0 To vDiaryDays.GetUpperBound(1)



                    vArray(0, lCount + (vArray.GetUpperBound(1) - vDiaryDays.GetUpperBound(1))) = vDiaryDays(1, lCount)



                    vArray(1, lCount + (vArray.GetUpperBound(1) - vDiaryDays.GetUpperBound(1))) = CInt(vDiaryDays(0, lCount))
                Next lCount
            End If

            m_lReturn = gPMFunctions.ShellSort2DArray(vArray, 1)

            Dim cboDueDate_NewIndex As Integer = -1
            cboDueDate_NewIndex = cboDueDate.Items.Add("")
            VB6.SetItemData(cboDueDate, 0, 0)

            For lCount As Integer = 0 To vArray.GetUpperBound(1)

                cboDueDate_NewIndex = cboDueDate.Items.Add(CStr(vArray(0, lCount)))

                VB6.SetItemData(cboDueDate, cboDueDate_NewIndex, CInt(vArray(1, lCount)))
            Next lCount



            '    m_lListIndex = -1
            m_lListIndex = 0

            ' Enabled/Disable Fields dependant on what we are doing.

            ' {* USER DEFINED CODE (Begin) *}


            Select Case Task
                ' Add a New Task Instance
                Case gPMConstants.PMEComponentAction.PMAdd
                    cboWrkTaskGroup.Enabled = True '(PMWrkTaskGroupId = 0)
                    cboWrkTask.Enabled = True '(PMWrkTaskId = 0)
                    cboWrkTask.ShowRequiresKeys = False
                    cboDueDate.Enabled = True
                    txtDueDate.Enabled = True
                    txtDueTime.Enabled = True
                    txtCustomer.Enabled = True
                    txtDescription.Enabled = True
                    chkIsUrgent.Enabled = True
                    chkIsComplete.Enabled = True
                    cboTaskUserGroup.Enabled = True
                    cboTaskUser.Enabled = True
                    cmdTaskLog.Enabled = False
                    cmdLinkObject.Enabled = True
                    m_bDirty = True
                    If m_sCallingAppName = "iPMUListRisks" Then
                        cboWrkTaskGroup.Enabled = False '(PMWrkTaskGroupId = 0)
                        cboWrkTask.Enabled = False '(PMWrkTaskId = 0)
                    End If

                    ' Edit the Details of an Existing Task Instance
                Case gPMConstants.PMEComponentAction.PMEdit
                    cboWrkTaskGroup.Enabled = False
                    cboWrkTask.Enabled = False
                    cboWrkTask.ShowRequiresKeys = True
                    cboDueDate.Enabled = True
                    txtDueDate.Enabled = True
                    txtDueTime.Enabled = True
                    txtCustomer.Enabled = True
                    txtDescription.Enabled = True
                    chkIsUrgent.Enabled = True
                    chkIsComplete.Enabled = False
                    cboTaskUserGroup.Enabled = False
                    cboTaskUser.Enabled = False
                    cmdLinkObject.Enabled = True

                    ' View an existing Task Instance
                Case gPMConstants.PMEComponentAction.PMView
                    cboWrkTaskGroup.Enabled = False
                    cboWrkTask.Enabled = False
                    cboWrkTask.ShowRequiresKeys = True
                    cboDueDate.Enabled = False
                    txtDueDate.Enabled = False
                    txtDueTime.Enabled = False
                    txtCustomer.Enabled = False
                    txtDescription.Enabled = False
                    chkIsUrgent.Enabled = False
                    chkIsComplete.Enabled = False
                    cboTaskUserGroup.Enabled = False
                    cboTaskUser.Enabled = False
                    cmdLinkObject.Enabled = False
                    AcceptButton = cmdTaskLog
                    ' ReAssign a Task Instance
                Case Else
                    cboWrkTaskGroup.Enabled = False
                    cboWrkTask.Enabled = False
                    cboWrkTask.ShowRequiresKeys = True
                    cboDueDate.Enabled = False
                    txtDueDate.Enabled = False
                    txtDueTime.Enabled = False
                    txtCustomer.Enabled = False
                    txtDescription.Enabled = False
                    chkIsUrgent.Enabled = False
                    chkIsComplete.Enabled = False
                    cboTaskUserGroup.Enabled = True
                    '        cboTaskUserGroup.SetFocus
                    cboTaskUser.Enabled = True
                    cmdLinkObject.Enabled = False

            End Select



            If m_bDisableCustomer Then
                txtCustomer.Enabled = False
                cboWrkTask.Enabled = True
                cboWrkTaskGroup.Enabled = True
            End If



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
    ' Name: DisplayCaptions
    '
    ' Description: Display all language specific captions.
    '
    ' ***************************************************************** '
    Private Function DisplayCaptions() As Integer

        Dim result As Integer = 0
        Try


            ' Display all language specific captions.

            '    Me.Caption = iPMFunc.GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACInterfaceTitle, _
            ''        iDataType:=PMResString)
            '
            '    ' Check for an error.
            '    If (Me.Caption = "") Then
            '        ' Failed to get data from the resource file.
            '        DisplayCaptions = PMFalse
            '
            '        ' Log Error.
            '        LogMessage _
            ''            iType:=PMLogError, _
            ''            sMsg:="Unable to retrieve data from the resource file." & Chr(10) & _
            ''            "Please check the file exists and the correct captions are available", _
            ''            vApp:=ACApp, _
            ''            vClass:=ACClass, _
            ''            vMethod:="DisplayCaptions"
            '
            '        Exit Function
            '    End If
            '
            '    cmdOK.Caption = iPMFunc.GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACOKButton, _
            ''        iDataType:=PMResString)
            '
            '    cmdCancel.Caption = iPMFunc.GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACCancelButton, _
            ''        iDataType:=PMResString)
            '
            '    cmdHelp.Caption = iPMFunc.GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACHelpButton, _
            ''        iDataType:=PMResString)
            '
            '    cmdNavigate.Caption = iPMFunc.GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACNavigateButton, _
            ''        iDataType:=PMResString)
            '
            '    tabMainTab.TabCaption(0) = iPMFunc.GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACTabTitle1, _
            ''        iDataType:=PMResString)

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetLookupValues
    '
    ' Description: Gets all of the lookup values, ready to be used by
    '              the lookup function.
    '
    ' ***************************************************************** '
    Private Function GetLookupValues() As Integer

        Dim result As Integer = 0
        Try


            ' Gets all of the lookup values.

            '    ' Check the task.
            '    Select Case (m_iTask)
            '        Case PMAdd
            '            ' Get all of the lookup values.
            '            m_lReturn& = m_oBusiness.GetLookupValues( _
            ''                iLookupType:=PMLookupAll, _
            ''                vTableArray:=m_vLookupValues, _
            ''                iLanguageID:=g_iLanguageID%, _
            ''                vResultArray:=m_vLookupDetails)
            '
            '        Case PMEdit
            '            ' Get all of the lookup values with the correct
            '            ' effective date.
            '            m_lReturn& = m_oBusiness.GetLookupValues( _
            ''                iLookupType:=PMLookupAllEffective, _
            ''                vTableArray:=m_vLookupValues, _
            ''                iLanguageID:=g_iLanguageID%, _
            ''                vResultArray:=m_vLookupDetails)
            '
            '        Case PMView
            '            ' Get lookup values for viewing only.
            '            m_lReturn& = m_oBusiness.GetLookupValues( _
            ''                iLookupType:=PMLookupSingle, _
            ''                vTableArray:=m_vLookupValues, _
            ''                iLanguageID:=g_iLanguageID%, _
            ''                vResultArray:=m_vLookupDetails)
            '    End Select
            '
            '    ' Check for errors.
            '    If (m_lReturn& <> PMTrue) Then
            '        GetLookupValues = PMFalse
            '
            '        ' Log Error.
            '        LogMessagePopup _
            ''            iType:=PMLogError, _
            ''            sMsg:="Failed to get the lookup values from the business object", _
            ''            vApp:=ACApp, _
            ''            vClass:=ACClass, _
            ''            vMethod:="GetLookupValues"
            '
            '        Exit Function
            '    End If

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup values", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetLookupDetails
    '
    ' Description: Gets all of the lookup details using the lookup
    '              values, then assigns them to the control passed.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (GetLookupDetails) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function GetLookupDetails(ByRef sLookupTable As String, ByRef ctlLookup As Control) As Integer
    '
    'Dim result As Integer = 0
    'Dim lRow, lCntr As Integer
    'Dim bFoundMatch As Boolean
    '
    ' Lookup value contants.
    'Const ACValueTableName As Integer = 0
    'Const ACValueID As Integer = 1
    'Const ACValueStartPos As Integer = 2
    'Const ACValueNumber As Integer = 3
    '
    ' Lookup detail contants.
    'Const ACDetailKey As Integer = 0
    'Const ACDetailDesc As Integer = 1
    '
    'Try 
    '
    '
    ' Get the lookup values.
    '
    '    bFoundMatch = False
    ''
    '    For lRow& = LBound(m_vLookupValues, 2) To UBound(m_vLookupValues, 2)
    '        ' Check for a match of the table name.
    '        If (Trim$(m_vLookupValues(ACValueTableName, lRow&)) = _
    ''        Trim$(sLookupTable$)) Then
    '            ' Found a match
    '            bFoundMatch = True
    '            Exit For
    '        End If
    '    Next lRow&
    ''
    '    ' Check if there has been a table match.
    '    If (bFoundMatch = False) Then
    '        GetLookupDetails = PMFalse
    ''
    '        ' Log Error.
    '        LogMessage _
    ''            iType:=PMLogOnError, _
    ''            sMsg:="Failed to get details for the table, " & sLookupTable$, _
    ''            vApp:=ACApp, _
    ''            vClass:=ACClass, _
    ''            vMethod:="GetLookupDetails"
    ''
    '        Exit Function
    '    End If
    ''
    '    ' Using the lookup values, populate the control with
    '    ' the details from the lookup details array.
    ''
    '    For lCntr& = m_vLookupValues(ACValueStartPos, lRow&) To _
    ''    (m_vLookupValues(ACValueStartPos, lRow&) + m_vLookupValues(ACValueNumber, lRow&)) - 1
    '        ' Add the details to the control.
    '        ctlLookup.AddItem m_vLookupDetails(ACDetailDesc, lCntr&)
    '        ctlLookup.ItemData(ctlLookup.NewIndex) = CLng(m_vLookupDetails(ACDetailKey, lCntr&))
    ''
    '        ' Check if this is the selected index.
    '        If (m_vLookupValues(ACValueID, lRow&) = _
    ''        m_vLookupDetails(ACDetailKey, lCntr&)) Then
    '            ctlLookup.ListIndex = ctlLookup.NewIndex
    '        End If
    '    Next lCntr&
    ''
    '    ' Check if the selected index is blank. If so,
    '    ' we set the controls index to zero.
    '    If (m_vLookupValues(ACValueID, lRow&) = "") Then
    '        ctlLookup.ListIndex = 0
    '    End If
    '
    'Return gPMConstants.PMEReturnCode.PMTrue
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error Section.
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

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

            m_lReturn = BusinessToInterface()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the details.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '    ' Not currently used by Voyager
            '    ' Display all of the lookup details.
            '    m_lReturn& = DisplayLookupDetails()
            '
            '    ' Check for errors.
            '    If (m_lReturn& <> PMTrue) Then
            '        GetInterfaceDetails = PMFalse
            '        Exit Function
            '    End If

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
                Case gPMConstants.PMEComponentAction.PMAdd
                    ' Check if form has been cancelled, if so,
                    ' prompt if you wish to lose details.
                    If m_lStatus = gPMConstants.PMEReturnCode.PMCancel Then
                        ' Get string messages

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
                        ' Form hasn't been cancelled, so we just go
                        ' ahead and add the details.

                        m_lReturn = InterfaceToBusiness()

                        ' Check for errors.
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' Failed to add the details
                            result = gPMConstants.PMEReturnCode.PMFalse

                            ' Log Error.
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to add the details", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand")
                        End If
                    End If

                Case gPMConstants.PMEComponentAction.PMEdit
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
                        ' Update the details using the business object.
                        m_lReturn = InterfaceToBusiness()

                        ' Check for errors.
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' Failed to update the details
                            result = gPMConstants.PMEReturnCode.PMFalse

                            ' Log Error.
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to update the details", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand")
                        End If
                    End If

                Case gPMConstants.PMEComponentAction.PMView
                    Return result

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
                        ' Update the details using the business object.
                        m_lReturn = InterfaceToBusiness()

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
    ' Name: ValidateTaskGroup
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Function ValidateTaskGroup() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue



            Return m_oBusiness.ValidateTaskGroup(cboWrkTaskGroup.ItemId)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateTaskGroup Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateTaskGroup", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: RefreshTaskList
    '
    ' Description: Shows the Tasks in the Selected Task Group
    '
    '
    ' ***************************************************************** '
    Private Sub RefreshTaskList(Optional ByVal v_iTaskId As Integer = 0)

        Try

            'cboWrkTask.FirstItem = "Any Task"
            cboWrkTask.PMTaskGroupID = cboWrkTaskGroup.ItemId

            If v_iTaskId > 0 Then
                cboWrkTask.DefaultTaskID = v_iTaskId
            Else
                cboWrkTask.DefaultTaskID = 0
            End If

            cboWrkTask.RefreshList()

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RefreshTaskList Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RefreshTaskList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: RefreshUserGroupList
    '
    ' Description: Shows the User Groups in the Selected Task Group
    '
    '
    ' ***************************************************************** '
    Private Sub RefreshUserGroupList(Optional ByVal v_iTaskGroupId As Integer = 0)

        Try

            'cboTaskUserGroup.FirstItem = "Any Task Group"
            cboTaskUserGroup.PMTaskGroupID = cboWrkTaskGroup.ItemId

            If v_iTaskGroupId > 0 Then
                cboTaskUserGroup.DefaultTaskGroupID = v_iTaskGroupId
            End If

            cboTaskUserGroup.RefreshList()

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RefreshUserGroupList Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RefreshUserGroupList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: RefreshUserList
    '
    ' Description: Shows the Users in the Selected User Group
    '
    '
    ' ***************************************************************** '
    Private Sub RefreshUserList(Optional ByVal v_iUserId As Integer = 0)

        Try

            cboTaskUser.FirstItem = "Any Group Member"
            cboTaskUser.PMUserGroupID = cboTaskUserGroup.UserGroupID
            If v_iUserId > 0 Then
                cboTaskUser.DefaultUserID = v_iUserId
            End If
            If m_iUserPartyKey > 0 Then
                cboTaskUser.PartyCnt = m_iUserPartyKey
            End If
            cboTaskUser.RefreshList()

        Catch excep As System.Exception




            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RefreshUserListFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="RefreshUserList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Function TaskLog() As Integer

        Dim result As Integer = 0
        'Dim oInterface As iPMWrkTaskInstLog.Interface_Renamed vikas commented
        Dim oInterface As iPMWrkTaskInstLog.Interface_Renamed

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'oInterface = New iPMWrkTaskInstLog.Interface_Renamed() vikas commented
            oInterface = New iPMWrkTaskInstLog.Interface_Renamed()

            'developers guide no.9
            'm_lReturn = CType(oInterface, SSP.S4I.Interfaces.ILocalInterface).Initialise()
            m_lReturn = oInterface.Initialise()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            oInterface.CallingAppName = ACApp

            m_lReturn = oInterface.SetProcessModes(vTask:=Task)

            oInterface.PMWrkTaskInstanceCnt = m_lPMWrkTaskInstanceCnt

            m_lReturn = oInterface.Start()

            oInterface.Dispose()

            oInterface = Nothing

            Return result

        Catch




            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function

    ' ***************************************************************** '
    '
    ' Name: AddLinkedKeys
    '
    ' Description:
    '
    ' History: 19/10/1999 DAK - Created.
    '
    ' AMB 21/01/2003 - Added workflow_information field - IAG Spec 229
    ' ***************************************************************** '
    Private Function AddLinkedKeys(ByVal v_vKeyArray(,) As Object) As Integer
        Dim result As Integer = 0


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' For Each Key in the Array
            For lRow As Integer = v_vKeyArray.GetLowerBound(1) To v_vKeyArray.GetUpperBound(1)

                ' Match by Name

                Select Case v_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lRow)
                    ' Task Description
                    Case PMNavKeyConst.PMKeyNameTaskDescription

                        Description = CStr(v_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        txtDescription.Text = Description

                        ' Task Customer
                    Case PMNavKeyConst.PMKeyNameTaskCustomer

                        Customer = CStr(v_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        txtCustomer.Text = Customer

                        ' Task Due Date
                    Case PMNavKeyConst.PMKeyNameTaskDueDate

                        DueDate = CDate(v_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        txtDueDate.Text = DateTimeHelper.ToString(DueDate)

                        ' Task Is Urgent
                    Case PMNavKeyConst.PMKeyNameTaskIsUrgent

                        IsUrgent = CInt(v_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        chkIsUrgent.CheckState = IsUrgent

                        ' AMB 21/01/2003 - Added workflow_information field - IAG Spec 229
                    Case PMNavKeyConst.PMKeyNameTaskWorkflowInformation

                        WorkflowInformation = CStr(v_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        txtWorkflowInfo.Text = WorkflowInformation

                        ' Not a Task Instance related Key.
                        ' Therefore must be a Task Inst Key
                    Case Else

                        ' Resize the Task Inst Key Array
                        If Information.IsArray(m_vTaskInstKeyArray) Then

                            ReDim Preserve m_vTaskInstKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, m_vTaskInstKeyArray.GetUpperBound(1) + 1)
                        Else
                            ReDim m_vTaskInstKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0)
                        End If

                        ' Add it to the end.



                        m_vTaskInstKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, m_vTaskInstKeyArray.GetUpperBound(1)) = v_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lRow)



                        m_vTaskInstKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, m_vTaskInstKeyArray.GetUpperBound(1)) = v_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)

                End Select

            Next lRow

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddLinkedKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddLinkedKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PRIVATE Methods (End)


    ' PRIVATE Events (Begin)

    ' {* GENERATED CODE (Begin) *}
    ' {* GENERATED CODE (End) *}

    ' PRIVATE Events (End)
    Private Sub cboDueDate_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboDueDate.SelectedIndexChanged

        If cboDueDate.SelectedIndex = m_lListIndex Then
            Exit Sub
        End If
        'PN 27272 14/03/06
        Select Case VB6.GetItemData(cboDueDate, cboDueDate.SelectedIndex)
            'DAK070999
            Case 1
                'Today
                m_dtTaskDueDate = DateTime.Today.AddDays(CDate("23:59:59").ToOADate())
            Case 2
                'Tomorrow
                m_dtTaskDueDate = DateTime.Today.AddDays(1).AddDays(CDate("23:59:59").ToOADate())
            Case 7
                'Within a week
                m_dtTaskDueDate = DateAndTime.DateAdd("ww", 1, DateTime.Today).AddDays(CDate("23:59:59").ToOADate())
            Case 31
                'Within a month
                m_dtTaskDueDate = DateTime.Today.AddMonths(1).AddDays(CDate("23:59:59").ToOADate())
            Case 90
                'Within a quarter
                m_dtTaskDueDate = DateAndTime.DateAdd("q", 1, DateTime.Today).AddDays(CDate("23:59:59").ToOADate())
            Case 365
                'Within a year
                m_dtTaskDueDate = DateTime.Today.AddYears(1).AddDays(CDate("23:59:59").ToOADate())
            Case Else
                '2005 User defined Diary Days
                m_dtTaskDueDate = DateTime.Today.AddDays(VB6.GetItemData(cboDueDate, cboDueDate.SelectedIndex)).AddDays(CDate("23:59:59").ToOADate())
        End Select

        m_lReturn = m_oFormFields.FormatControl(txtDueDate, m_dtTaskDueDate)
        m_lReturn = m_oFormFields.FormatControl(txtDueTime, m_dtTaskDueDate)

        m_lListIndex = cboDueDate.SelectedIndex

        m_bDirty = True

    End Sub

    Private Sub cboTaskUser_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboTaskUser.Click

        m_bDirty = True

    End Sub

    Private Sub cboTaskUserGroup_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboTaskUserGroup.Click
        ' Show the Users in the Selected Group

        RefreshUserList()

        m_bDirty = True

    End Sub

    Private Sub cboWrkTask_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboWrkTask.Click

        m_bDirty = True

    End Sub

    Private Sub cboWrkTaskGroup_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboWrkTaskGroup.Click

        ' Show the Tasks in the Selected Group

        If Task = gPMConstants.PMEComponentAction.PMAdd Then
            If m_bFormLoaded Then
                m_lReturn = ValidateTaskGroup()
                'Set here as the cboWrkTask dropdown needs to be set to 0 when the form is already loaded.
                m_lPMWrkTaskId = 0
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("Warning - This task group is not completely set up." & Strings.Chr(13) & Strings.Chr(10) & "It lacks either tasks or user groups.", "Invalid Selection", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    cboWrkTaskGroup.ItemId = m_lTaskGroup

                    Exit Sub
                End If
            End If
        End If

        m_lTaskGroup = cboWrkTaskGroup.ItemId
        If m_lPMWrkTaskId <= 0 Then
            m_lPMWrkTaskId = 0
        End If
        'm_lPMWrkTaskId = 0
        RefreshTaskList(m_lPMWrkTaskId)
        'NIIT Comment: In Reminder/Memo Task Combo "Task: cboWrkTask " Not Polulated with Task
        'm_lPMWrkTaskId = 0

        RefreshUserGroupList()

        m_bDirty = True

    End Sub

    Private Sub cmdLinkObject_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdLinkObject.Click
        Dim vKeyArray As Object
        Dim oComponent, oNav3 As aPMNav.NavigatorV3
        Dim lStatus As gPMConstants.PMEReturnCode


        Dim sComponent As String = LinkedObjectName & "." & LinkedClassName

        Try


            m_lReturn = g_oObjectManager.GetInstance(oComponent, sComponent, gPMConstants.PMGetLocalInterface)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get instance of " & sComponent, vApp:=ACApp, vClass:=ACClass, vMethod:="cmdLinkObject_Click")
                Exit Sub
            End If

            oNav3 = oComponent

            oNav3.CallingAppName = ACApp

            oNav3.PMAuthorityLevel = PMAuthorityLevel

            m_lReturn = oNav3.Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Start Component :- " & sComponent, vApp:=ACApp, vClass:=ACClass, vMethod:="cmdLinkObject_Click")

                'Modified as per VB code
                'oComponent.Terminate()
                ReflectionHelper.Invoke(oComponent, "Dispose", New Object() {})

                oComponent = Nothing
                oNav3 = Nothing
                Exit Sub
            End If

            lStatus = oNav3.Status

            m_lReturn = oNav3.GetKeys(vKeyArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get keys from " & sComponent, vApp:=ACApp, vClass:=ACClass, vMethod:="cmdLinkObject_Click")

                'Modified as per VB code
                ' oComponent.Terminate()
                ReflectionHelper.Invoke(oComponent, "Dispose", New Object() {})
                oComponent = Nothing
                oNav3 = Nothing
                Exit Sub
            End If

            ' Terminate the Component

            'Modified as per VB code
            'oComponent.Terminate()
            ReflectionHelper.Invoke(oComponent, "Dispose", New Object() {})
            oComponent = Nothing
            oNav3 = Nothing

            If lStatus = gPMConstants.PMEReturnCode.PMOK Then

                LinkedKeysAdded = True

                If Information.IsArray(vKeyArray) Then

                    ' Add the keys

                    m_lReturn = AddLinkedKeys(vKeyArray)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to set keys from " & sComponent, vApp:=ACApp, vClass:=ACClass, vMethod:="cmdLinkObject_Click")
                        Exit Sub
                    End If

                End If

            End If

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdLinkObject_Click Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdLinkObject_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)


            'Modified as per VB code
            'oComponent.Terminate()
            ReflectionHelper.Invoke(oComponent, "Dispose", New Object() {})
            oComponent = Nothing
            oNav3 = Nothing

            Exit Sub

        End Try

    End Sub

    Private Sub cmdTaskLog_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdTaskLog.Click

        m_lReturn = TaskLog()

    End Sub

    Private Sub frmInterface_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm = eventSender

            With uctPMResizer1
                .SetControlResizeOption("cmdOK", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROPositionOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                .SetControlResizeOption("cmdCancel", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROPositionOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                .SetControlResizeOption("cmdHelp", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROPositionOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)

                .SetControlResizeOption("cmdNavigate", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROTopOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)

                .SetControlResizeOption("tabMainTab", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROSizeOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)

                .SetControlResizeOption("cmdTaskLog", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROPositionOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)

                .SetControlResizeOption("cmdLinkObject", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROPositionOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)

            End With

        End If
    End Sub

    Private Sub Form_Initialize_Renamed()
        iPMFunc.ShowFormInTaskBar_Attach()
    End Sub


    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        iPMFunc.ShowFormInTaskBar_Detach()

        With uctPMResizer1
            .NoResizeByDefault = True
            .FormMinHeight = 6645
            .FormMinWidth = 9405
        End With
        'The following commented code is shifted to 'Load_Renamed' function of  'frmInterface' 
        'm_lReturn = SetFieldValidation()
        'modified,for getting focused 
        cboTaskUser.Select()
        'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
        '    Status = gPMConstants.PMEReturnCode.PMFalse
        '    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
        '    Exit Sub
        'End If
        cboWrkTaskGroup.Focus()
        cboWrkTaskGroup.Select()

        m_oBusiness.GetUserPartyCnt(g_oObjectManager.UserID, m_iUserPartyKey)
        RefreshUserList()
    End Sub
    Public Sub LoadTaskInstanceInterface()
        ShowFormInTaskBar_Detach()

        With uctPMResizer1
            .NoResizeByDefault = True
            .FormMinHeight = 6645
            .FormMinWidth = 9405
        End With

        m_lReturn = SetFieldValidation()

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Status = gPMConstants.PMEReturnCode.PMFalse
            'Developer Guide No.180
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            Exit Sub
        End If

    End Sub
    Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        ' Forms query unload event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.


            If UnloadMode <> vbFormCode Then

                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = ProcessCommand()

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    Cancel = 1
                    eventArgs.Cancel = True
                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Sub
                End If
            End If

            ' Terminate the business object

            m_oBusiness.Dispose()
            ' Destroy the instance of the business object
            ' from memory.
            m_oBusiness = Nothing

            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception



            ' Error Section.

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

            eventArgs.Cancel = Cancel <> 0
        End Try

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        ' Click event of the OK button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            If (txtDueDate.Text.Trim() = "") And txtDueDate.Enabled Then
                m_lReturn = MessageBox.Show("This is a mandatory field. You must enter data in this field", "Mandatory Field - Due Date", MessageBoxButtons.OK, MessageBoxIcon.Error)
                txtDueDate.Focus()
                Exit Sub
            End If
            If CDate(txtDueDate.Text.Trim()) < DateTime.Today And txtDueDate.Enabled Then
                m_lReturn = MessageBox.Show("Due Date can not be earlier than today ", "Valid - Due Date", MessageBoxButtons.OK, MessageBoxIcon.Error)
                txtDueDate.Focus()
                Exit Sub
            End If

            If (txtCustomer.Text.Trim() = "") And txtCustomer.Enabled Then
                m_lReturn = MessageBox.Show("This is a mandatory field. You must enter data in this field", "Mandatory Field - Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
                txtCustomer.Focus()
                Exit Sub
            End If

            If (txtDescription.Text.Trim() = "") And txtDescription.Enabled Then
                m_lReturn = MessageBox.Show("This is a mandatory field. You must enter data in this field", "Mandatory Field - Description", MessageBoxButtons.OK, MessageBoxIcon.Error)
                txtDescription.Focus()
                Exit Sub
            End If

            If Task = gPMConstants.PMEComponentAction.PMAdd Then
                If cboWrkTask.ListIndex = -1 Then
                    m_lReturn = MessageBox.Show("This is a mandatory field. You must enter data in this field", "Mandatory Field - Task", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    cboWrkTask.Focus()
                    Exit Sub
                End If
            End If

            If Task = gPMConstants.PMEComponentAction.PMAdd Then
                If cboTaskUserGroup.ListIndex = -1 Then
                    m_lReturn = MessageBox.Show("This is a mandatory field. You must enter data in this field", "Mandatory Field - User Group", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    cboTaskUserGroup.Focus()
                    Exit Sub
                End If
            End If

            If Task = gPMConstants.PMEComponentAction.PMAdd Then
                If cboTaskUser.ListIndex = -1 Then
                    m_lReturn = MessageBox.Show("This is a mandatory field. You must enter data in this field", "Mandatory Field - User", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    cboTaskUser.Focus()
                    Exit Sub
                End If
            End If

            'DAK131099
            If Task = gPMConstants.PMEComponentAction.PMAdd And LinkedCaption <> "" And Not LinkedKeysAdded Then
                m_lReturn = MessageBox.Show("Press " & LinkedCaption & " Button to add Linked Object", Application.ProductName)
                cmdLinkObject.Focus()
                Exit Sub
            End If

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = ProcessCommand()

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
            m_lReturn = ProcessCommand()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            Else
                Return
            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdNavigate_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNavigate.Click

        ' Click event of the Navigate button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMNavigate

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = ProcessCommand()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Navigate command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNavigate_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private isInitializingComponent As Boolean
    Private Sub txtCustomer_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCustomer.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If

        m_bDirty = True

    End Sub

    Private Sub txtCustomer_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCustomer.Enter

        m_lReturn = m_oFormFields.GotFocus(txtCustomer)

    End Sub

    Private Sub txtCustomer_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCustomer.Leave

        m_lReturn = m_oFormFields.LostFocus(txtCustomer)

    End Sub

    Private Sub txtDescription_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDescription.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If

        m_bDirty = True

    End Sub

    Private Sub txtDescription_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDescription.Enter

        m_lReturn = m_oFormFields.GotFocus(txtDescription)

    End Sub

    Private Sub txtDescription_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDescription.Leave

        m_lReturn = m_oFormFields.LostFocus(txtDescription)

    End Sub

    Private Sub txtDueDate_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDueDate.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If

        m_bDirty = True

    End Sub

    Private Sub txtDueDate_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDueDate.Enter

        m_lReturn = m_oFormFields.GotFocus(txtDueDate)

    End Sub

    Private Sub txtDueDate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDueDate.Leave

        m_lReturn = m_oFormFields.LostFocus(txtDueDate)

    End Sub

    Private Sub txtDueTime_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDueTime.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If

        m_bDirty = True

    End Sub

    Private Sub txtDueTime_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDueTime.Enter

        m_lReturn = m_oFormFields.GotFocus(txtDueTime)

    End Sub

    Private Sub txtDueTime_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDueTime.Leave

        m_lReturn = m_oFormFields.LostFocus(txtDueTime)

    End Sub
    Private Sub frmInterface_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed
        MemoryHelper.ReleaseMemory()
    End Sub

    Private Sub tabMainTab_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles tabMainTab.KeyDown
        'Developer Guide No 293
        If e.Alt And e.KeyCode = Keys.D1 Then
            tabMainTab.SelectedIndex = 0
        End If
        If e.Alt And e.KeyCode = Keys.D2 Then
            tabMainTab.SelectedIndex = 1
        End If
    End Sub

End Class
