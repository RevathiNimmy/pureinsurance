Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
'Developer Guide No.129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")> _
Public NotInheritable Class Interface_Renamed
    Implements IDisposable
    Implements SSP.S4I.Interfaces.ILocalInterface
    ' ***************************************************************** '
    ' Class Name: Interface
    '
    ' Date: 26/10/1998
    '
    ' Description: Main public class to accompany the interface form.
    '
    ' Edit History:
    ' DAK131099 - changes for licencing and linked objects
    ' AMB 21/01/2003 - Added workflow_information field - IAG Spec 229
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "Interface"


    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Interface Form
    Private m_frmInterface As frmInterface

    ' RDC 31072002
    Private m_frmMultiTasksForm As frmAssignMultipleTask

    'DAK080800
    ' LinkedObject - no longer used - retained for binary compatibility
    Private m_oComponent As Object

    ' Object parameter members.
    Private m_sCallingAppName As String = ""

    Private m_lPMAuthorityLevel As Integer

    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_sNavigatorTitle As String = ""

    Private m_lPMWrkTaskInstanceCnt As Integer
    Private m_vPMWrkTaskInstanceCntArray() As Object ' RAM20020715 : Multiple Tasks
    Private m_lPMWrkTaskGroupId As Integer
    Private m_lPMWrkTaskId As Integer
    Private m_sCustomer As String = ""
    Private m_dtTaskDueDate As Date
    Private m_lPmuserGroupID As Integer
    Private m_iUserID As Integer
    ' RDC 16082002
    Private m_lUserGroupID As Integer
    Private m_sDescription As String = ""
    Private m_iTaskStatus As Integer
    Private m_iIsUrgent As Integer
    Private m_iIsSystemTask As Integer
    Private m_iTypeOfTask As Integer
    Private m_lPMNavProcessID As Integer
    Private m_sComponentObjectName As String = ""
    Private m_sComponentClassName As String = ""
    Private m_lAutoDeleteAfterNumDays As Integer
    Private m_dtDateCreated As Date
    Private m_iCreatedByID As Integer
    ' AMB 20/01/2003 - Add workflow information property
    Private m_sWorkflowInformation As String = ""
    ' RDC 10012002
    Private m_sNavXMLfile As String = ""

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
    ' LinkedKeysAdded
    Private m_bLinkedKeysAdded As Boolean

    ' Task Instance keys.
    ' These can be supplied via the SetKeys mechanism.
    Private m_vTaskInstKeyArray As Object

    ' Stores the exit status of the interface.
    Private m_lStatus As Integer

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer
    Private m_bDisableCustomer As Integer
    Private m_lKeepOnTop As Integer
    Private m_iIsTaskReview As Integer
    Private m_iSourceId As Integer
    ' {* USER DEFINED CODE (Begin) *}


    ' {* USER DEFINED CODE (End) *}

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public Property DisableCustomer() As Boolean
        Get
            Return m_bDisableCustomer
        End Get
        Set(ByVal Value As Boolean)
            m_bDisableCustomer = Value
        End Set
    End Property


    Public WriteOnly Property PMAuthorityLevel() As Integer
        Set(ByVal Value As Integer)
            m_lPMAuthorityLevel = Value
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
    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusArchitecture
        End Get
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Standard Property.

            ' Set the calling application name.
            m_sCallingAppName = Value

        End Set
    End Property

    Public ReadOnly Property Status() As Integer
        Get

            ' Standard Property.

            ' Return the interface exit status.
            Return m_lStatus

        End Get
    End Property

    Public ReadOnly Property Task() As Integer
        Get

            ' Standard Property.

            ' Return the task.
            Return m_iTask

        End Get
    End Property

    Public ReadOnly Property Navigate() As Integer
        Get

            ' Standard Property.

            ' Return the navigate flag.
            Return m_lNavigate

        End Get
    End Property

    Public ReadOnly Property ProcessMode() As Integer
        Get

            ' Standard Property.

            ' Return the process mode.
            Return m_lProcessMode

        End Get
    End Property

    Public ReadOnly Property TransactionType() As String
        Get

            ' Standard Property.

            ' Return the type of business.
            Return m_sTransactionType

        End Get
    End Property

    Public ReadOnly Property EffectiveDate() As Date
        Get

            ' Standard Property.

            ' Return the effective date.
            Return m_dtEffectiveDate

        End Get
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

    ' RAM20020715 : Multiple Tasks
    ' RAM20020715 : Multiple Tasks
    Public Property PMWrkTaskInstanceCntArray() As Object
        Get
            Return VB6.CopyArray(m_vPMWrkTaskInstanceCntArray)
        End Get
        Set(ByVal Value As Object)
            m_vPMWrkTaskInstanceCntArray = Value
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
            Return m_sCustomer.Trim()
        End Get
        Set(ByVal Value As String)
            m_sCustomer = Value.Trim()
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

    ' RDC 16082002
    Public Property UserGroupID() As Integer
        Get
            Return m_lUserGroupID
        End Get
        Set(ByVal Value As Integer)
            m_lUserGroupID = Value
        End Set
    End Property

    Public Property Description() As String
        Get
            Return m_sDescription.Trim()
        End Get
        Set(ByVal Value As String)
            m_sDescription = Value.Trim()
        End Set
    End Property

    ' RDC 10012003 Navigator XM file
    Public Property NavXMLfile() As String
        Get
            Return m_sNavXMLfile
        End Get
        Set(ByVal Value As String)
            m_sNavXMLfile = Value
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

    Public Property IsUrgent() As Integer
        Get
            Return m_iIsUrgent
        End Get
        Set(ByVal Value As Integer)
            m_iIsUrgent = Value
        End Set
    End Property

    Public ReadOnly Property IsSystemTask() As Integer
        Get
            Return m_iIsSystemTask
        End Get
    End Property

    Public ReadOnly Property TypeOfTask() As Integer
        Get
            Return m_iTypeOfTask
        End Get
    End Property

    Public ReadOnly Property PMNavProcessId() As Integer
        Get
            Return m_lPMNavProcessID
        End Get
    End Property

    Public ReadOnly Property ComponentObjectName() As String
        Get
            Return m_sComponentObjectName.Trim()
        End Get
    End Property

    Public ReadOnly Property ComponentClassName() As String
        Get
            Return m_sComponentClassName.Trim()
        End Get
    End Property

    Public ReadOnly Property AutoDeleteAfterNumDays() As Integer
        Get
            Return m_lAutoDeleteAfterNumDays
        End Get
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

    Public Property Component() As Object
        Get
            'DAK080800 - no longer used - retained for binary compatibility
            Return m_oComponent
        End Get
        Set(ByVal Value As Object)
            If Microsoft.VisualBasic.Information.IsReference(Value) And Not (TypeOf Value Is String) Then
                'DAK080800 - no longer used - retained for binary compatibility
                m_oComponent = Value
            Else
                'DAK080800 - no longer used - retained for binary compatibility
                m_oComponent = Value
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
    Public Property SourceId() As Integer
        Get
            Return m_iSourceId
        End Get
        Set(ByVal Value As Integer)
            m_iSourceId = Value
        End Set
    End Property

    ' {* USER DEFINED CODE (End) *}
    ' PUBLIC Property Procedures (End)


    ' PRIVATE Property Procedures (Begin)
    ' PRIVATE Property Procedures (End)


    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise() As Integer Implements SSP.S4I.Interfaces.ILocalInterface.Initialise

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the object manager.
            g_oObjectManager = New bObjectManager.ObjectManager()

            ' Call the initialise method.
            m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to call the initialise method.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Set the object manager to nothing.
                g_oObjectManager = Nothing

                If m_lReturn <> gPMConstants.PMEReturnCode.PMCancel Then

                    ' Log Error.
                    gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the object manager", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                End If

                Return result
            End If

            ' Store the language ID from the object manager
            ' to the public variables, to enable us to use
            ' them throughout the object.
            With g_oObjectManager
                g_iLanguageID = .LanguageID
                g_iSourceID = .SourceID
            End With

            ' Initialise the process modes.
            m_iTask = gPMConstants.PMEComponentAction.PMView
            m_lNavigate = gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep)

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
                If g_oObjectManager IsNot Nothing Then
                    g_oObjectManager.Dispose()
                    g_oObjectManager = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub


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

    ' ***************************************************************** '
    ' Name: Start (Standard Method)
    '
    ' Description: Entry point for the object to start its processing.
    '
    ' ***************************************************************** '
    Public Function Start() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Starts the interface processing.
            m_lReturn = ProcessInterface()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' PUBLIC Methods (End)

    ' FRIEND Methods (Begin)
    ' ***************************************************************** '
    ' Name: SetKeys (Standard Method)
    '
    ' Description: Stores all of the parameter members with the key
    '              array.
    '
    ' ***************************************************************** '
    Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sTaskCode, sTaskGroupCode As String
        Dim lTaskGroupID, lTaskID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check we have a valid array.
            If Not Information.IsArray(vKeyArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_vTaskInstKeyArray = Nothing
            sTaskGroupCode = ""
            sTaskCode = ""

            ' For Each Key in the Array
            For lRow As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)

                ' Match by Name

                Select Case vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lRow)
                    ' Task Instance Cnt
                    Case PMNavKeyConst.PMKeyNameTaskInstanceCnt

                        PMWrkTaskInstanceCnt = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                        ' Task Group Code
                    Case PMNavKeyConst.PMKeyNameTaskGroupCode

                        sTaskGroupCode = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)).Trim()

                        ' Task Group ID
                    Case PMNavKeyConst.PMKeyNameTaskGroupID

                        PMWrkTaskGroupId = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                        ' Task Code
                    Case PMNavKeyConst.PMKeyNameTaskCode

                        sTaskCode = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)).Trim()

                        ' Task ID
                    Case PMNavKeyConst.PMKeyNameTaskID

                        PMWrkTaskId = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                        ' Task Description
                    Case PMNavKeyConst.PMKeyNameTaskDescription

                        Description = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                        ' Task Customer
                    Case PMNavKeyConst.PMKeyNameTaskCustomer

                        Customer = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                        ' Task Due Date
                    Case PMNavKeyConst.PMKeyNameTaskDueDate

                        DueDate = CDate(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                        ' Task Is Urgent
                    Case PMNavKeyConst.PMKeyNameTaskIsUrgent

                        IsUrgent = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                        '**************
                        ' MEvans : 07-04-2003 : Issue 3262
                    Case PMNavKeyConst.PMKeyNameUserGroupID

                        m_lPmuserGroupID = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                        ' User ID
                    Case PMNavKeyConst.PMKeyNameUserID

                        m_iUserID = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                        ' Task Created Date
                    Case "task_created_date"

                        m_dtDateCreated = CDate(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                        ' Task Created by User ID
                    Case "task_created_by_id"

                        m_iCreatedByID = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        '**************

                        ' AMB 21/01/2003 - Added workflow_information field - IAG Spec 229
                        ' Task Workflow Information
                    Case PMNavKeyConst.PMKeyNameKeepWindowOnTop

                        m_lKeepOnTop = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        ' AMB 21/01/2003 - Added workflow_information field - IA
                    Case PMNavKeyConst.PMKeyNameTaskWorkflowInformation

                        WorkflowInformation = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

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



                        m_vTaskInstKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, m_vTaskInstKeyArray.GetUpperBound(1)) = vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lRow)



                        m_vTaskInstKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, m_vTaskInstKeyArray.GetUpperBound(1)) = vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)

                End Select

            Next lRow

            ' If we have been supplied a Task Group or Task Code,
            ' get the ID for them.
            If (sTaskGroupCode <> "") Or (sTaskCode <> "") Then
                GetIDsFromCodes(v_sTaskGroupCode:=sTaskGroupCode, v_sTaskCode:=sTaskCode, r_lTaskGroupID:=lTaskGroupID, r_lTaskID:=lTaskID)
                If lTaskGroupID > 0 Then
                    PMWrkTaskGroupId = lTaskGroupID
                End If
                If lTaskID > 0 Then
                    PMWrkTaskId = lTaskID
                End If
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetKeys (Standard Method)
    '
    ' Description: Stores all of the key array with the parameter
    '              members.
    '
    ' ***************************************************************** '
    Friend Function GetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try


            '    ReDim vKeyArray(0, PMKeyValue)
            '    vKeyArray(0, PMKeyName) = PMKeyNameTaskInstanceCnt
            '    vKeyArray(0, PMKeyValue) = m_lPMWrkTaskInstanceCnt

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' FRIEND Methods (End)

    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: GetIDsFromCodes
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Sub GetIDsFromCodes(ByVal v_sTaskGroupCode As String, ByVal v_sTaskCode As String, ByRef r_lTaskGroupID As Integer, ByRef r_lTaskID As Integer)
        Dim oPMLookup As bPMLookup.Business



        r_lTaskGroupID = 0
        r_lTaskID = 0

        Dim temp_oPMLookup As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_oPMLookup, "bPMLookup.Business", vinstancemanager:=gPMConstants.PMGetViaClientManager)
        oPMLookup = temp_oPMLookup
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If


        oPMLookup.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusArchitecture

        If v_sTaskGroupCode.Trim() <> "" Then

            m_lReturn = oPMLookup.GetEffectiveIDFromCode(v_sTableName:="PMWrk_task_group", v_sCode:=v_sTaskGroupCode, v_dtEffectiveDate:=DateTime.Now, r_lID:=r_lTaskGroupID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                r_lTaskGroupID = 0
            End If
        End If

        If v_sTaskCode.Trim() <> "" Then

            m_lReturn = oPMLookup.GetEffectiveIDFromCode(v_sTableName:="PMWrk_Task", v_sCode:=v_sTaskCode, v_dtEffectiveDate:=DateTime.Now, r_lID:=r_lTaskID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                r_lTaskID = 0
            End If
        End If


        oPMLookup.Dispose()
        oPMLookup = Nothing


    End Sub

    ' ***************************************************************** '
    ' Name: ProcessInterface (Standard Method)
    '
    ' Description: Calls the appropriate methods to process the
    '              interface.
    '
    ' Edit History :
    ' RAM20020715  : Code added to check for any Multiple Tasks Assignment
    ' ***************************************************************** '
    Private Function ProcessInterface() As Integer

        Dim result As Integer = 0
        Dim intLoadForm As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' RAM20020715 : Check if the multiple Tasks are sent in
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        If Information.IsArray(m_vPMWrkTaskInstanceCntArray) Then

            m_frmMultiTasksForm = New frmAssignMultipleTask()

            m_lReturn = LoadMutiTaskAssignForm()
            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to load the interface.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Display the interface.
            ' RDC 10092002 FormShown will be true if error occured during form.BusinessToInterface
            If Not m_frmMultiTasksForm.FormShown Then
                m_frmMultiTasksForm.ShowDialog()
            End If

            If m_frmMultiTasksForm.ErrorNumber <> 0 Then
                result = m_frmMultiTasksForm.ErrorNumber
            End If

            ' Fetch any needed valuse


            ' Unload and destroy the instance of the interface
            ' from memory.
            m_frmMultiTasksForm.Close()
            m_frmMultiTasksForm = Nothing


        Else
            ' Do As normal
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            'Developer Guide No. 
            ' Commented the below line and instantiated in the "LoadInterface" method
            'm_frmInterface = New frmInterface()
            intLoadForm = 1

            ' Load the interface into memory.
            m_lReturn = LoadInterface()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to load the interface.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Display the interface.
            m_lReturn = ShowInterface(lDisplayState:=FormShowConstants.Modal)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to display the inteface.
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Destroy the interface from memory.
            m_lReturn = UnLoadInterface()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to unload the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' RAM20020715 : Check if the multiple Tasks are sent in
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        End If
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: LoadInterface (Standard Method)
    '
    ' Description: Loads the instance of the interface into memory and
    '              passes the parameters in.
    '
    ' AMB 20/01/2003 - Add workflow information property
    ' ***************************************************************** '
    Private Function LoadInterface() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        'Developer Guide No. 
        m_frmInterface = New frmInterface()
        ' Assign the parameters to the interface properties.
        With m_frmInterface
            .CallingAppName = m_sCallingAppName
            .Task = m_iTask
            .Navigate = m_lNavigate
            .ProcessMode = m_lProcessMode
            .TransactionType = m_sTransactionType
            .EffectiveDate = m_dtEffectiveDate
            ' {* USER DEFINED CODE (Begin) *}
            ' Set the Task Instance Cnt
            .PMWrkTaskInstanceCnt = PMWrkTaskInstanceCnt
            .PMWrkTaskId = m_lPMWrkTaskId
            .PMWrkTaskGroupId = m_lPMWrkTaskGroupId

            .Customer = Customer
            .Description = Description
            .DueDate = DueDate
            .PMUserGroupID = PMUserGroupID
            .UserID = UserID
            .IsUrgent = IsUrgent
            .TaskCreatedDate = m_dtDateCreated
            .TaskCreatedByID = m_iCreatedByID
            ' AMB 20/01/2003
            .WorkflowInformation = WorkflowInformation

            .IsTaskReview = m_iIsTaskReview
            ' Set any Task Instance Keys that we have been supplied with
            '.set_TaskInstKeyArray(DefaultPropHelper.GetDefaultProperty(m_vTaskInstKeyArray)) vikas commented
            .TaskInstKeyArray = m_vTaskInstKeyArray
            .DisableCustomer = m_bDisableCustomer
            ' {* USER DEFINED CODE (End) *}
        End With

        ' Load the instance of the interface into memory.

        m_frmInterface.LoadTaskInstanceInterface()

        ' Check if we have had an error so far.
        If m_frmInterface.ErrorNumber = gPMConstants.PMEReturnCode.PMError Then
            ' We have already encountered an error,
            ' so we MUST return the error.
            result = m_frmInterface.ErrorNumber
        End If

        ' m_lReturn = CType(m_frmInterface, SSP.S4I.Interfaces.ILocalInterface).Initialise()
        'Developer Guide No. 
        m_lReturn = m_frmInterface.Initialise
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: UnLoadInterface (Standard Method)
    '
    ' Description: Unloads the instance of the interface from memory.
    '
    ' AMB 20/01/2003 - Add workflow information property
    ' ***************************************************************** '
    Private Function UnLoadInterface() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Assign the property members from the interface parameters.
        With m_frmInterface
            m_lStatus = .Status

            ' {* USER DEFINED CODE (Begin) *}
            m_lPMWrkTaskInstanceCnt = .PMWrkTaskInstanceCnt
            m_lPMWrkTaskGroupId = .PMWrkTaskGroupId
            m_lPMWrkTaskId = .PMWrkTaskId
            m_sCustomer = .Customer
            m_dtTaskDueDate = .DueDate
            m_lPmuserGroupID = .PMUserGroupID
            m_iUserID = .UserID
            m_sDescription = .Description
            m_iTaskStatus = .TaskStatus
            m_iIsUrgent = .IsUrgent
            m_iIsSystemTask = .IsSystemTask
            m_iTypeOfTask = .TypeOfTask
            m_lPMNavProcessID = .PMNavProcessId
            m_sComponentObjectName = .ComponentObjectName
            m_sComponentClassName = .ComponentClassName
            m_lAutoDeleteAfterNumDays = .AutoDeleteAfterNumDays
            ' {* USER DEFINED CODE (End) *}
            ' AMB 20/01/2003
            m_dtDateCreated = .TaskCreatedDate
            m_iCreatedByID = .TaskCreatedByID
            m_sWorkflowInformation = .WorkflowInformation
            m_sNavXMLfile = .NavXMLfile
        End With

        ' Unload and destroy the instance of the interface
        ' from memory.
        m_frmInterface.Close()
        m_frmInterface = Nothing

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: ShowInterface (Standard Method)
    '
    ' Description: Displays the instance of the interface using the
    '              display state.
    '
    ' ***************************************************************** '
    Private Function ShowInterface(ByRef lDisplayState As Integer) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        If m_lKeepOnTop = 1 Then
            m_lReturn = iPMFunc.SetWindowPlacement(m_frmInterface.Handle.ToInt32(), True)
        End If

        ' Display the interface.
        ' VB6.ShowForm(m_frmInterface, lDisplayState)

        lDisplayState = m_frmInterface.ShowDialog()
        'Developer Guide No. 
        If lDisplayState = FormShowConstants.Modal Then
            ' Check for any form errors.
            If m_frmInterface.ErrorNumber <> 0 Then
                result = m_frmInterface.ErrorNumber
            End If
        End If

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: AddLinkedKeys
    '
    ' Description:
    '
    ' History: 19/10/1999 DAK - Created.
    '
    'DAK080800 - no longer used - retained for binary compatibility
    ' All code has been moved to the form
    ' ***************************************************************** '
    Public Function AddLinkedKeys() As Integer


        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddLinkedKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddLinkedKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'PRIVATE Methods (End)

    Public Sub New()
        MyBase.New()

        ' Class Initialise Event.

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        'Catch excep As System.Exception
        '
        '
        '
        ' Error Section.
        '
        ' Log Error Message
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface entry class", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialise", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    ' ***************************************************************** '
    ' Name : LoadMutiTaskAssignForm
    ' Description: Loads the instance of the Multiple Scheduled Task interface
    '               into memory and passes the parameters in.
    ' Edit History
    ' RAM20020715 : Created
    ' ***************************************************************** '
    Private Function LoadMutiTaskAssignForm() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Assign the parameters to the interface properties.
        With m_frmMultiTasksForm
            .CallingAppName = m_sCallingAppName
            .Task = m_iTask
            .Navigate = m_lNavigate
            .ProcessMode = m_lProcessMode
            .TransactionType = m_sTransactionType
            .EffectiveDate = m_dtEffectiveDate
            'Modified  refer developer guide no. 24 (Latest Guide)
            '.set_PMWrkTaskInstanceCntArray(PMWrkTaskInstanceCntArray)
            .PMWrkTaskInstanceCntArray = PMWrkTaskInstanceCntArray

            ' RDC 16082002
            .UserID = m_iUserID
            .UserGroupID = m_lUserGroupID
            ' {* USER DEFINED CODE (End) *}
        End With

        ' Load the instance of the interface into memory.

        'Load(m_frmMultiTasksForm) 
        ' m_frmMultiTasksForm.Show()

        ' Check if we have had an error so far.
        If m_frmMultiTasksForm.ErrorNumber = gPMConstants.PMEReturnCode.PMError Then
            ' We have already encountered an error,
            ' so we MUST return the error.
            result = m_frmMultiTasksForm.ErrorNumber
        End If

        m_lReturn = m_frmMultiTasksForm.Initialise()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function
End Class

