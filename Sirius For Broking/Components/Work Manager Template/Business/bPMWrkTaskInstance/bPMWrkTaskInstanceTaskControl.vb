Option Strict Off
Option Explicit On
'developer guide no 129. 
'Start
Imports SSP.Shared

'End

<System.Runtime.InteropServices.ProgId("TaskControl_NET.TaskControl")> _
Public NotInheritable Class TaskControl
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: TaskControl
    '
    ' Date: 26/10/1998
    '
    ' Description:
    '
    '
    ' Edit History:
    ' DAK141299 - Add IsVisible column to Task Instance
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 11/12/2003
    Private m_sUsername As String = ""

    Private m_sPassword As String = ""

    Private m_iUserID As Integer

    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    ' ************************************************


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "TaskControl"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Business Methods Class
    Private m_oBusiness As bPMWrkTaskInstanceTemp.Business

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
    Public Function Initialise(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel


            ' Initialisation Code.

            ' Set Username and Password

            ' Set User ID

            ' Set Calling Application

            ' Set Language ID

            ' Set Source ID

            ' Set Currency ID

            ' Set Log Level

            ' Check the Supplied Database.

            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create Task Control
            m_oBusiness = New bPMWrkTaskInstanceTemp.Business()
            m_lReturn = CType(m_oBusiness.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=sCallingAppName, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)
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
    ' ***************************************************************** '
    'DAK141299
    Public Function CreateNew(ByVal v_lPMWrkTaskGroupID As Integer, ByVal v_lPMWrkTaskID As Integer, ByVal v_sCustomer As String, ByVal v_dtTaskDueDate As Date, ByVal v_lPMUserGroupID As Integer, ByVal v_sDescription As String, ByVal v_iTaskStatus As Integer, ByVal v_iIsUrgent As Integer, ByRef r_lPMWrkTaskInstanceCnt As Integer, Optional ByVal v_iUserID As Integer = 0, Optional ByVal v_vKeyArray(,) As Object = Nothing, Optional ByVal v_iIsVisible As Integer = gPMConstants.PMEReturnCode.PMTrue) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a New Task Instance Class
            'DAK141299

            m_lReturn = CType(m_oBusiness.CreateNew(v_lPMWrkTaskGroupID:=v_lPMWrkTaskGroupID, v_lPMWrkTaskID:=v_lPMWrkTaskID, v_sCustomer:=v_sCustomer, v_dtTaskDueDate:=v_dtTaskDueDate, v_lPMUserGroupID:=v_lPMUserGroupID, v_sDescription:=v_sDescription, v_iTaskStatus:=v_iTaskStatus, v_iIsUrgent:=v_iIsUrgent, r_lPMWrkTaskInstanceCnt:=r_lPMWrkTaskInstanceCnt, v_iUserID:=v_iUserID, v_vKeyArray:=v_vKeyArray, v_iIsVisible:=v_iIsVisible), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
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
    '
    ' ***************************************************************** '
    Public Function GetDetails(ByVal v_lPMWrkTaskInstanceCnt As Integer, ByRef r_lPMWrkTaskGroupID As Integer, ByRef r_lPMWrkTaskID As Integer, ByRef r_sCustomer As String, ByRef r_dtTaskDueDate As Date, ByRef r_lPMUserGroupID As Integer, ByRef r_iUserID As Integer, ByRef r_sDescription As String, ByRef r_iTaskStatus As Integer, ByRef r_iIsUrgent As Integer, ByRef r_dtDateCreated As Date, ByRef r_iCreatedByID As Integer, ByRef r_dtLastModified As Date, ByRef r_iModifiedByID As Integer, Optional ByRef r_vIsVisible As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(m_oBusiness.GetDetails(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt, r_lPMWrkTaskGroupID:=r_lPMWrkTaskGroupID, r_lPMWrkTaskID:=r_lPMWrkTaskID, r_sCustomer:=r_sCustomer, r_dtTaskDueDate:=r_dtTaskDueDate, r_lPMUserGroupID:=r_lPMUserGroupID, r_iUserID:=r_iUserID, r_sDescription:=r_sDescription, r_iTaskStatus:=r_iTaskStatus, r_iIsUrgent:=r_iIsUrgent, r_dtDateCreated:=r_dtDateCreated, r_iCreatedByID:=r_iCreatedByID, r_dtLastModified:=r_dtLastModified, r_iModifiedByID:=r_iModifiedByID, r_vIsVisible:=r_vIsVisible), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
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
    ' Name: GetTaskInstKeys
    '
    ' Description: Gets all of the Keys for a Single Task Instance.
    '
    ' ***************************************************************** '
    Public Function GetTaskInstKeys(ByVal v_lPMWrkTaskInstanceCnt As Integer, ByRef r_vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(m_oBusiness.GetTaskInstKeys(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt, r_vKeyArray:=r_vKeyArray), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetTaskInstKeysFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTaskInstKeys", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: AmendDetails
    '
    ' Description: Amend the Task Details.
    '
    ' ***************************************************************** '
    Public Function AmendDetails(ByVal v_lPMWrkTaskInstanceCnt As Integer, ByVal v_sCustomer As String, ByVal v_dtTaskDueDate As Date, ByVal v_sDescription As String, ByVal v_iIsUrgent As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(m_oBusiness.AmendDetails(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt, v_sCustomer:=v_sCustomer, v_dtTaskDueDate:=v_dtTaskDueDate, v_sDescription:=v_sDescription, v_iIsUrgent:=v_iIsUrgent), gPMConstants.PMEReturnCode)
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
    ' Name: Assign
    '
    ' Description: Assigns the new Task to a specific user.
    '
    ' Note: The Group must have already been specified when the Task was
    '       created.
    ' ***************************************************************** '
    Public Function Assign(ByVal v_lPMWrkTaskInstanceCnt As Integer, ByVal v_iUserID As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(m_oBusiness.Assign(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt, v_iUserID:=v_iUserID), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AssignFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="Assign", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ReAssign
    '
    ' Description: Reassign the Task to another Group or specific User.
    '
    ' ***************************************************************** '
    Public Function ReAssign(ByVal v_lPMWrkTaskInstanceCnt As Integer, ByVal v_lPMUserGroupID As Integer, Optional ByVal v_iUserID As Integer = 0) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(m_oBusiness.ReAssign(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt, v_lPMUserGroupID:=v_lPMUserGroupID, v_iUserID:=v_iUserID), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
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
    ' Name: SetStatusComplete
    '
    ' Description: Mark a Task as SetStatusCompleted.
    '
    ' ***************************************************************** '
    Public Function SetStatusComplete(ByVal v_lPMWrkTaskInstanceCnt As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(m_oBusiness.SetStatusComplete(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetStatusCompleteFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetStatusComplete", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetStatusInComplete
    '
    ' Description: Reset a Task from Complete.
    '
    '
    ' ***************************************************************** '
    Public Function SetStatusInComplete(ByVal v_lPMWrkTaskInstanceCnt As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(m_oBusiness.SetStatusInComplete(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetStatusInCompleteFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetStatusInComplete", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetStatusInProgress
    '
    ' Description: SetStatusInProgress the Task.
    '
    '
    ' ***************************************************************** '
    Public Function SetStatusInProgress(ByVal v_lPMWrkTaskInstanceCnt As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(m_oBusiness.SetStatusInProgress(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetStatusInProgressFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetStatusInProgress", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Delete
    '
    ' Description: Delete a Task and associated bits.
    '
    '
    ' ***************************************************************** '
    Public Function Delete(ByVal v_lPMWrkTaskInstanceCnt As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(m_oBusiness.Delete(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="Delete", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: AutoDelete
    '
    ' Description: Automatically delete Completed Tasks.
    ' ***************************************************************** '
    Public Function AutoDelete() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Call the Auto Delete method in the Business.
            m_lReturn = m_oBusiness.AutoDelete()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AutoDeleteFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoDelete", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: CreateNewByCode
    '
    ' Description: Create a new task instance using the Task Code rather
    '              than task id.
    '
    ' History: 22/06/2000 DAK - Created.
    '
    ' ***************************************************************** '
    Public Function CreateNewByCode(ByVal v_lPMWrkTaskGroupID As Integer, ByVal v_sPMWrkTaskCode As String, ByVal v_sCustomer As String, ByVal v_dtTaskDueDate As Date, ByVal v_lPMUserGroupID As Integer, ByVal v_sDescription As String, ByVal v_iTaskStatus As Integer, ByVal v_iIsUrgent As Integer, ByRef r_lPMWrkTaskInstanceCnt As Integer, Optional ByVal v_iUserID As Integer = 0, Optional ByVal v_vKeyArray(,) As Object = Nothing, Optional ByVal v_iIsVisible As Integer = gPMConstants.PMEReturnCode.PMTrue) As Integer
        Dim result As Integer = 0
        Dim lPMWrkTaskID As Integer


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(GetPMWrkTaskID(v_sPMWrkTaskCode:=v_sPMWrkTaskCode, r_lPMWrkTaskID:=lPMWrkTaskID), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Create a New Task Instance Class

            m_lReturn = CType(m_oBusiness.CreateNew(v_lPMWrkTaskGroupID:=v_lPMWrkTaskGroupID, v_lPMWrkTaskID:=lPMWrkTaskID, v_sCustomer:=v_sCustomer, v_dtTaskDueDate:=v_dtTaskDueDate, v_lPMUserGroupID:=v_lPMUserGroupID, v_sDescription:=v_sDescription, v_iTaskStatus:=v_iTaskStatus, v_iIsUrgent:=v_iIsUrgent, r_lPMWrkTaskInstanceCnt:=r_lPMWrkTaskInstanceCnt, v_iUserID:=v_iUserID, v_vKeyArray:=v_vKeyArray, v_iIsVisible:=v_iIsVisible), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateNewByCode Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateNewByCode", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    '
    ' Name: GetPMWrkTaskID
    '
    ' Description: Returns the task id for a given task code
    '
    ' History: 22/06/2000 DAK - Created.
    '
    ' ***************************************************************** '
    Private Function GetPMWrkTaskID(ByVal v_sPMWrkTaskCode As String, ByRef r_lPMWrkTaskID As Integer) As Integer
        Dim result As Integer = 0
        Dim oTask As bPMTask.Business




        result = gPMConstants.PMEReturnCode.PMTrue

        ' Create Component Services

        ' Create bPMTask.Business

        oTask = New bPMTask.Business
        m_lReturn = oTask.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            oTask = Nothing
            Return m_lReturn
        End If

        ' Get the required task id

        m_lReturn = oTask.GetDataByCode(v_sPMWrkTaskCode:=v_sPMWrkTaskCode, r_vTaskId:=r_lPMWrkTaskID)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            oTask.Dispose()
            oTask = Nothing
            Return m_lReturn
        End If


        oTask.Dispose()
        oTask = Nothing

        Return result

    End Function

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

End Class
