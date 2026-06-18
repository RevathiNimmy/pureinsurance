Option Strict Off
Option Explicit On
'developer guide no.129
Imports SSP.Shared
<System.Runtime.InteropServices.ProgId("FormClass_NET.FormClass")>
Public NotInheritable Class FormClass
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: FormClass
    '
    ' Date: 26/10/1998
    '
    ' Description:
    '
    '
    ' Edit History:
    ' DAK141299 - Add is_visible column to task instance
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 18/09/2003
    ' Username.
    Private m_sUsername As String = ""

    ' Password.
    Private m_sPassword As String = ""

    ' User ID
    Private m_iUserID As Integer

    ' Calling Application
    Private m_sCallingAppName As String = ""
    ' Source ID
    Private m_iSourceID As Integer
    ' Language ID
    Private m_iLanguageID As Integer
    ' Currency ID
    Private m_iCurrencyID As Integer
    ' LogLevel
    Private m_iLogLevel As Integer
    ' ************************************************

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "FormClass"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Error Code (Private)
    Private m_lReturn As Integer

    ' Key of the Task Instance we are working with.
    Private m_lPmwrkTaskInstanceCnt As Integer

    ' Business Methods Class
    Private m_oBusiness As bPMWrkTaskInstance.Business

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)
    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusArchitecture
        End Get
    End Property

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
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long

        ' RDC 13062002 CompServ replaced with BAS module
        'Dim oCS As sPMServerCS.PMServerBusinessCS

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            ' Set Username and Password
            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel


            ' Check the Supplied Database.

            m_lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase)

            '    Set oCS = Nothing

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create Task Control
            m_oBusiness = New bPMWrkTaskInstance.Business()
            m_lReturn = m_oBusiness.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=sCallingAppName, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
                If m_oBusiness IsNot Nothing Then
                    m_oBusiness.Dispose()
                    m_oBusiness = Nothing
                End If
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                End If
                m_oDatabase = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: CreateNew
    '
    ' Description: Creates a New Task Instance.
    '
    ' AMB 20/01/2003 - workflow_information column added
    ' ***************************************************************** '
    'DAK141299
    Public Function CreateNew(ByVal v_lPMWrkTaskGroupID As Integer, ByVal v_lPMWrkTaskID As Integer, ByVal v_sCustomer As String, ByVal v_dtTaskDueDate As Date, ByVal v_lPMUserGroupID As Integer, ByVal v_sDescription As String, ByVal v_iTaskStatus As Integer, ByVal v_iIsUrgent As Integer, ByVal v_dtDateCreated As Date, ByVal v_iCreatedByID As Integer, ByRef r_lPMWrkTaskInstanceCnt As Integer, Optional ByVal v_sWorkflowInformation As String = "", Optional ByVal v_iUserID As Integer = 0, Optional ByVal v_vKeyArray(,) As Object = Nothing, Optional ByVal v_iIsVisible As Integer = gPMConstants.PMEReturnCode.PMTrue, Optional ByVal v_iIsTaskReview As Integer = 0) As Integer
        ' AMB 20/01/2003

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a New Task Instance Class
            'DAK141299

            m_lReturn = m_oBusiness.CreateNew(v_lPMWrkTaskGroupID:=v_lPMWrkTaskGroupID, v_lPMWrkTaskID:=v_lPMWrkTaskID, v_sCustomer:=v_sCustomer, v_dtTaskDueDate:=v_dtTaskDueDate, v_lPMUserGroupID:=v_lPMUserGroupID, v_sDescription:=v_sDescription, v_iTaskStatus:=v_iTaskStatus, v_iIsUrgent:=v_iIsUrgent, r_lPMWrkTaskInstanceCnt:=r_lPMWrkTaskInstanceCnt, v_dtDateCreated:=v_dtDateCreated, v_iCreatedByID:=v_iCreatedByID, v_iUserID:=v_iUserID, v_vKeyArray:=v_vKeyArray, v_iIsVisible:=v_iIsVisible, v_sWorkflowInformation:=v_sWorkflowInformation, v_iIsTaskReview:=v_iIsTaskReview)
            ' AMB 20/01/2003

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateNewFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateNew", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDetails
    '
    ' Description: Gets the details for the Task Instance.
    '
    ' AMB 20/01/2003 - workflow_information column added
    ' ***************************************************************** '
    Public Function GetDetails(ByVal v_lPMWrkTaskInstanceCnt As Integer, ByRef r_lPMWrkTaskGroupID As Integer, ByRef r_lPMWrkTaskID As Integer, ByRef r_sCustomer As String, ByRef r_dtTaskDueDate As Date, ByRef r_lPMUserGroupID As Integer, ByRef r_iUserID As Integer, ByRef r_sDescription As String, ByRef r_iTaskStatus As Integer, ByRef r_iIsUrgent As Integer, ByRef r_dtDateCreated As Date, ByRef r_iCreatedByID As Integer, ByRef r_dtLastModified As Date, ByRef r_iModifiedByID As Integer, Optional ByRef r_sWorkflowInformation As String = "", Optional ByRef r_vIsVisible As Object = Nothing, Optional ByRef r_iIsTaskReview As Integer = 0) As Integer
        ' AMB 20/01/2003

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_oBusiness.GetDetails(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt, r_lPMWrkTaskGroupID:=r_lPMWrkTaskGroupID, r_lPMWrkTaskID:=r_lPMWrkTaskID, r_sCustomer:=r_sCustomer, r_dtTaskDueDate:=r_dtTaskDueDate, r_lPMUserGroupID:=r_lPMUserGroupID, r_iUserID:=r_iUserID, r_sDescription:=r_sDescription, r_iTaskStatus:=r_iTaskStatus, r_iIsUrgent:=r_iIsUrgent, r_dtDateCreated:=r_dtDateCreated, r_iCreatedByID:=r_iCreatedByID, r_dtLastModified:=r_dtLastModified, r_iModifiedByID:=r_iModifiedByID, r_vIsVisible:=r_vIsVisible, r_sWorkflowInformation:=r_sWorkflowInformation, r_iIsTaskReview:=r_iIsTaskReview)
            ' AMB 20/01/2003

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDetailsFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetTaskDetails
    '
    ' Description: Gets the details for the Task itself.
    ' ***************************************************************** '
    Public Function GetTaskDetails(ByVal v_lPMWrkTaskID As Integer, ByRef r_iIsSystemTask As Integer, ByRef r_iTypeOfTask As Integer, ByRef r_lPMNavProcessID As Integer, ByRef r_sComponentObjectName As String, ByRef r_sComponentClassName As String, ByRef r_lAutoDeleteAfterNumDays As Integer, ByRef r_lDisplayIcon As Integer, ByRef r_iIsViewOnlyTask As Integer, ByRef r_sLinkedObjectName As String, ByRef r_sLinkedClassName As String, ByRef r_sLinkedCaption As String, ByRef r_iIsAvailableTask As Integer) As Integer
        Return GetTaskDetails(v_lPMWrkTaskID:=v_lPMWrkTaskID, r_iIsSystemTask:=r_iIsSystemTask, r_iTypeOfTask:=r_iTypeOfTask, r_lPMNavProcessID:=r_lPMNavProcessID, r_sComponentObjectName:=r_sComponentObjectName, r_sComponentClassName:=r_sComponentClassName, r_lAutoDeleteAfterNumDays:=r_lAutoDeleteAfterNumDays, r_lDisplayIcon:=r_lDisplayIcon, r_iIsViewOnlyTask:=r_iIsViewOnlyTask, r_sLinkedObjectName:=r_sLinkedObjectName, r_sLinkedClassName:=r_sLinkedClassName, r_sLinkedCaption:=r_sLinkedCaption, r_iIsAvailableTask:=r_iIsAvailableTask, r_sNavXMLfile:="", r_sTaskDescription:="")
    End Function

    Public Function GetTaskDetails(ByVal v_lPMWrkTaskID As Integer, ByRef r_iIsSystemTask As Integer, ByRef r_iTypeOfTask As Integer, ByRef r_lPMNavProcessID As Integer, ByRef r_sComponentObjectName As String, ByRef r_sComponentClassName As String, ByRef r_lAutoDeleteAfterNumDays As Integer, ByRef r_lDisplayIcon As Integer, ByRef r_iIsViewOnlyTask As Integer, ByRef r_sLinkedObjectName As String, ByRef r_sLinkedClassName As String, ByRef r_sLinkedCaption As String, ByRef r_iIsAvailableTask As Integer, ByRef r_sNavXMLfile As String) As Integer
        Return GetTaskDetails(v_lPMWrkTaskID:=v_lPMWrkTaskID, r_iIsSystemTask:=r_iIsSystemTask, r_iTypeOfTask:=r_iTypeOfTask, r_lPMNavProcessID:=r_lPMNavProcessID, r_sComponentObjectName:=r_sComponentObjectName, r_sComponentClassName:=r_sComponentClassName, r_lAutoDeleteAfterNumDays:=r_lAutoDeleteAfterNumDays, r_lDisplayIcon:=r_lDisplayIcon, r_iIsViewOnlyTask:=r_iIsViewOnlyTask, r_sLinkedObjectName:=r_sLinkedObjectName, r_sLinkedClassName:=r_sLinkedClassName, r_sLinkedCaption:=r_sLinkedCaption, r_iIsAvailableTask:=r_iIsAvailableTask, r_sNavXMLfile:=r_sNavXMLfile, r_sTaskDescription:="")
    End Function

    Public Function GetTaskDetails(ByVal v_lPMWrkTaskID As Integer, ByRef r_iIsSystemTask As Integer, ByRef r_iTypeOfTask As Integer, ByRef r_lPMNavProcessID As Integer, ByRef r_sComponentObjectName As String, ByRef r_sComponentClassName As String, ByRef r_lAutoDeleteAfterNumDays As Integer, ByRef r_lDisplayIcon As Integer, ByRef r_iIsViewOnlyTask As Integer, ByRef r_sLinkedObjectName As String, ByRef r_sLinkedClassName As String, ByRef r_sLinkedCaption As String, ByRef r_iIsAvailableTask As Integer, ByRef r_sNavXMLfile As String, ByRef r_sTaskDescription As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_oBusiness.GetTaskDetails(v_lPMWrkTaskID:=v_lPMWrkTaskID, r_iIsSystemTask:=r_iIsSystemTask, r_iTypeOfTask:=r_iTypeOfTask, r_lPMNavProcessID:=r_lPMNavProcessID, r_sComponentObjectName:=r_sComponentObjectName, r_sComponentClassName:=r_sComponentClassName, r_lAutoDeleteAfterNumDays:=r_lAutoDeleteAfterNumDays, r_lDisplayIcon:=r_lDisplayIcon, r_iIsViewOnlyTask:=r_iIsViewOnlyTask, r_sLinkedObjectName:=r_sLinkedObjectName, r_sLinkedClassName:=r_sLinkedClassName, r_sLinkedCaption:=r_sLinkedCaption, r_iIsAvailableTask:=r_iIsAvailableTask, r_sNavXMLfile:=r_sNavXMLfile, r_sTaskDescription:=r_sTaskDescription)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetTaskDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTaskDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: AmendDetails
    '
    ' Description: Amend the Task Details.
    '
    ' AMB 21/01/2003 - Added workflow_information field - IAG Spec 229
    ' ***************************************************************** '
    Public Function AmendDetails(ByVal v_lPMWrkTaskInstanceCnt As Integer, ByVal v_sCustomer As String, ByVal v_dtTaskDueDate As Date, ByVal v_sDescription As String, ByVal v_iIsUrgent As Integer, ByVal v_dtLastModified As Date, ByVal v_iModifiedByID As Integer) As Integer
        Return AmendDetails(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt, v_sCustomer:=v_sCustomer, v_dtTaskDueDate:=v_dtTaskDueDate, v_sDescription:=v_sDescription, v_iIsUrgent:=v_iIsUrgent, v_dtLastModified:=v_dtLastModified, v_iModifiedByID:=v_iModifiedByID, v_sWorkflowInformation:="", v_iIsTaskReview:=0)
    End Function

    Public Function AmendDetails(ByVal v_lPMWrkTaskInstanceCnt As Integer, ByVal v_sCustomer As String, ByVal v_dtTaskDueDate As Date, ByVal v_sDescription As String, ByVal v_iIsUrgent As Integer, ByVal v_dtLastModified As Date, ByVal v_iModifiedByID As Integer, ByVal v_sWorkflowInformation As String) As Integer
        Return AmendDetails(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt, v_sCustomer:=v_sCustomer, v_dtTaskDueDate:=v_dtTaskDueDate, v_sDescription:=v_sDescription, v_iIsUrgent:=v_iIsUrgent, v_dtLastModified:=v_dtLastModified, v_iModifiedByID:=v_iModifiedByID, v_sWorkflowInformation:=v_sWorkflowInformation, v_iIsTaskReview:=0)
    End Function

    Public Function AmendDetails(ByVal v_lPMWrkTaskInstanceCnt As Integer, ByVal v_sCustomer As String, ByVal v_dtTaskDueDate As Date, ByVal v_sDescription As String, ByVal v_iIsUrgent As Integer, ByVal v_dtLastModified As Date, ByVal v_iModifiedByID As Integer, ByVal v_iIsTaskReview As Integer) As Integer
        Return AmendDetails(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt, v_sCustomer:=v_sCustomer, v_dtTaskDueDate:=v_dtTaskDueDate, v_sDescription:=v_sDescription, v_iIsUrgent:=v_iIsUrgent, v_dtLastModified:=v_dtLastModified, v_iModifiedByID:=v_iModifiedByID, v_sWorkflowInformation:="", v_iIsTaskReview:=v_iIsTaskReview)
    End Function

    Public Function AmendDetails(ByVal v_lPMWrkTaskInstanceCnt As Integer, ByVal v_sCustomer As String, ByVal v_dtTaskDueDate As Date, ByVal v_sDescription As String, ByVal v_iIsUrgent As Integer, ByVal v_dtLastModified As Date, ByVal v_iModifiedByID As Integer, ByVal v_sWorkflowInformation As String, ByVal v_iIsTaskReview As Integer) As Integer
        ' AMB 21/01/2003

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_oBusiness.AmendDetails(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt, v_sCustomer:=v_sCustomer, v_dtTaskDueDate:=v_dtTaskDueDate, v_sDescription:=v_sDescription, v_iIsUrgent:=v_iIsUrgent, v_dtLastModified:=v_dtLastModified, v_iModifiedByID:=v_iModifiedByID, v_sWorkflowInformation:=v_sWorkflowInformation, v_iIsTaskReview:=v_iIsTaskReview)
            ' AMB 21/01/2003

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AmendDetailsFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="AmendDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ReAssign
    '
    ' Description: Reassign the Task to another Group or specific User.
    '
    '
    ' ***************************************************************** '
    Public Function ReAssign(ByVal v_lPMWrkTaskInstanceCnt As Integer, ByVal v_lPMUserGroupID As Integer) As Integer
        Return ReAssign(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt, v_lPMUserGroupID:=v_lPMUserGroupID, v_iUserID:=0)
    End Function

    Public Function ReAssign(ByVal v_lPMWrkTaskInstanceCnt As Integer, ByVal v_lPMUserGroupID As Integer, ByVal v_iUserID As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_oBusiness.ReAssign(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt, v_lPMUserGroupID:=v_lPMUserGroupID, v_iUserID:=v_iUserID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ReAssignFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="ReAssign", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ValidateTaskGroup
    '
    ' Description: Checks that a Task group has associated bits.
    '
    '
    ' ***************************************************************** '
    Public Function ValidateTaskGroup(ByVal v_lPMWrkTaskGroupID As Integer) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                ' Clear the Database Parameters Collection
                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="pmwrk_task_group_id", vValue:=CStr(v_lPMWrkTaskGroupID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                ' Execute SQL Statement
                m_lReturn = .SQLSelect(sSQL:=ACValidateTaskGroupSQL, sSQLName:=ACValidateTaskGroupName, bStoredProcedure:=ACValidateTaskGroupStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' How many records were selected
                lRecordCount = .Records.Count()

                ' Do we have any records ?
                If lRecordCount = 1 Then
                    ' Selected, No action required
                Else
                    Return gPMConstants.PMEReturnCode.PMNotFound
                End If

            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateTaskGroup Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateTaskGroup", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)
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
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    ' ***************************************************************** '
    ' Name: ReAssignMultipleTask
    '
    ' Description: Reassign Multiple Tasks to another Group or specific User.
    '
    ' Edit History
    ' RAM20020715 : Created
    ' ***************************************************************** '
    Public Function ReAssignMultipleTask(ByVal v_vPMWrkTaskInstanceCntArray As Object, ByVal v_lPMUserGroupID As Integer) As Integer
        Return ReAssignMultipleTask(v_vPMWrkTaskInstanceCntArray:=v_vPMWrkTaskInstanceCntArray, v_lPMUserGroupID:=v_lPMUserGroupID, v_iUserID:=0)
    End Function

    Public Function ReAssignMultipleTask(ByVal v_vPMWrkTaskInstanceCntArray As Object, ByVal v_lPMUserGroupID As Integer, ByVal v_iUserID As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_oBusiness.ReAssignMultipleTask(v_vPMWrkTaskInstanceCntArray:=v_vPMWrkTaskInstanceCntArray, v_lPMUserGroupID:=v_lPMUserGroupID, v_iUserID:=v_iUserID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ReAssignMultipleTask", vApp:=ACApp, vClass:=ACClass, vMethod:="ReAssignMultipleTask", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' *****************************************************************
    ' Name: GetGroupUsers
    '
    ' Description: Get users in supplied group
    '
    ' Edit History
    ' RDC 16082002 created
    ' *****************************************************************
    Public Function GetGroupUsers(ByVal lUsergroupID As Integer, ByRef vGroupUsers(,) As Object) As Int32

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Try


            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="UserGroupID", vValue:=CStr(lUsergroupID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetUsersForGroupSQL, sSQLName:=ACGetUsersForGroupName, bStoredProcedure:=ACGetUsersForGroupStored, vResultArray:=vGroupUsers)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or Not Informations.IsArray(vGroupUsers) Then
                Return result
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetGroupUsers Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetGroupUsers", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' *****************************************************************
    ' Name: CheckIsSupervisor
    '
    ' Description: check if the user is a supervisor in the
    '              supplied user group
    '
    ' Edit History
    ' RDC 06092002 created
    ' *****************************************************************
    Public Function CheckIsSupervisor(ByVal iUserID As Integer, ByVal lPmuserGroupID As Integer, ByRef iIsSupervisor As Integer) As Integer

        Dim result As Integer = 0
        Dim iIsSysAdmin As Integer
        Dim vValue As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            m_lReturn = CheckIsSysAdmin(iUserID, iIsSysAdmin)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            If iIsSysAdmin Then
                iIsSupervisor = 1
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="UserID", vValue:=CStr(iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="UserGroupID", vValue:=CStr(lPmuserGroupID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="IsSupervisor", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACCheckIsSupervisorSQL, sSQLName:=ACCheckIsSupervisorName, bStoredProcedure:=ACCheckIsSupervisorStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            vValue = m_oDatabase.Parameters.Item("IsSupervisor").Value


            If Convert.IsDBNull(vValue) Or Informations.IsNothing(vValue) Then
                iIsSupervisor = 0
            Else
                iIsSupervisor = vValue
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch



            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function

    ' *****************************************************************
    ' Name: CheckIsSysAdmin
    '
    ' Description: check if the user is in a user group defined
    '              as a Sys Admin group
    '
    ' Edit History
    ' RDC 13092002 created
    ' *****************************************************************
    Private Function CheckIsSysAdmin(ByVal iUserID As Integer, ByRef iIsSysAdmin As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing



        result = gPMConstants.PMEReturnCode.PMFalse

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=CStr(iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If
        'developer guide no. 40
        m_lReturn = m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=DateTime.Now, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACCheckIsSysAdminSQL, sSQLName:=ACCheckIsSysAdminName, bStoredProcedure:=ACCheckIsSysAdminStored, vResultArray:=vResultArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If


        iIsSysAdmin = vResultArray(0, 0)


        Return gPMConstants.PMEReturnCode.PMTrue


    End Function

    ' *****************************************************************
    ' Name: GetTaskUserGroups
    '
    ' Description: Get list of user groups that can perform this task.
    '  Routine gets all user groups that are common to the list of
    '  supplied tasks. The routine is repeatedly called with a new task
    '  ID. The first time it is called, vUserGroups is populated with
    '  all user groups capable of processing the task. On subsequent
    '  calls, vUserGroups is returned populated with only common user
    '  groups. After the final call, vUserGroups will be populated with
    '  only user groups capable of processing ALL supplied tasks.
    '
    ' Edit History
    ' RDC 09092002 created
    ' *****************************************************************
    Public Function GetTaskUserGroups(ByVal lTaskID As Integer, ByRef vUserGroups(,) As Object) As Integer

        Dim result As Integer = 0
        Static bFirst As Boolean

        Dim bFound As Boolean
        Dim vResultArray(,) As Object = Nothing
        Dim vMatchArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            m_oDatabase.Parameters.Clear()

            ' task ID
            m_lReturn = m_oDatabase.Parameters.Add(sName:="TaskID", vValue:=CStr(lTaskID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            ' get the user groups capable of processing this task
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetTaskUserGroupsSQL, sSQLName:=ACGetTaskUserGroupsName, bStoredProcedure:=ACGetTaskUserGroupsStored, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' SQL failed
                Return result
            End If

            If Not Informations.IsArray(vResultArray) Then
                ' failed to get any user groups
                Return result
            End If

            If Not Informations.IsArray(vUserGroups) Then
                If bFirst Then
                    ' we've already been here, so there are no common user groups
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                bFirst = True

                ' first time this routine has been called, so just copy resultarray and exit

                vUserGroups = vResultArray
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            ' search for user groups common to both arrays

            For iResult As Integer = vResultArray.GetLowerBound(1) To vResultArray.GetUpperBound(1)

                bFound = False

                If Informations.IsArray(vUserGroups) Then
                    For iUserGroup As Integer = vUserGroups.GetLowerBound(1) To vUserGroups.GetUpperBound(1)



                        If vResultArray(0, iResult).Equals(vUserGroups(0, iUserGroup)) Then
                            ' it's in the array
                            bFound = True
                            Exit For
                        End If

                    Next
                End If

                If bFound Then
                    ' 'either it's found, or it's the first time this routine's been called
                    If Not Informations.IsArray(vMatchArray) Then
                        ReDim vMatchArray(1, 0)
                    Else

                        ReDim Preserve vMatchArray(1, vMatchArray.GetUpperBound(1) + 1)
                    End If




                    vMatchArray(0, vMatchArray.GetUpperBound(1)) = vResultArray(0, iResult)



                    vMatchArray(1, vMatchArray.GetUpperBound(1)) = vResultArray(1, iResult)
                End If
            Next

            ' common user groups now in temp array, so return that temp array ready for next call

            vUserGroups = vMatchArray


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch



            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function
    ' *****************************************************************
    ' Name: GetDiaryDays
    '
    ' Edit History
    ' ECK Created 8/4/2005 2005 Diary Tasks
    ' *****************************************************************
    Public Function GetDiaryDays(ByRef vDiaryDays(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse


            ' get the user groups capable of processing this task
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetDiaryDaysSQL, sSQLName:=ACGetDiaryDaysName, bStoredProcedure:=ACGetDiaryDaysStored, vResultArray:=vDiaryDays)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' SQL failed
                Return result
            End If



            Return result

        Catch



            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function


    Public Function GetUserPartyCnt(ByVal iUserID As Integer, ByRef iPartyCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=CStr(iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetUserPartyCntSQL, sSQLName:=ACGetUserPartyCntName, bStoredProcedure:=ACGetUserPartyCntStored, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If


            iPartyCnt = vResultArray(0, 0)


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch



            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function
End Class

