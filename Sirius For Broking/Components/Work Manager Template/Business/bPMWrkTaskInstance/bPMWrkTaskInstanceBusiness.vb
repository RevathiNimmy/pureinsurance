Option Strict Off
Option Explicit On
'developer guide no 129.
'Start
Imports SSP.Shared
'End
Friend NotInheritable Class Business
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 30/10/1998
    '
    ' Description:
    '
    '
    ' Edit History:
    ' DAK121099 - Changes for Product Licencing and linked objects
    ' DAK141299 - Add is_visible column to task instance
    ' DAK231299 - Replace Task Group Category with Task Category
    ' DAK240100 - correct the array processing in CreateNew
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Business"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Error Code (Private)
    Private m_lReturn As Integer

    ' Task Instance
    Private m_oTaskInstance As bPMWrkTaskInstanceTemp.PMWrkInstance

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
    Private m_bRestrictedTaskView As Boolean
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
        Dim bMultiTreeAcc, bRestrictedTaskView As Boolean
        'Developer Guie No 17
        Dim vValue As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Initialisation Code.

            ' Set Username and Password

            ' Set User ID

            ' Set Calling Application

            ' Set Language ID

            ' Set Source ID

            ' Set Currency ID

            ' Set Log Level
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

            ' Check the Supplied Database.

            m_lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oTaskInstance = New bPMWrkTaskInstanceTemp.PMWrkInstance()
            m_lReturn = m_oTaskInstance.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=sCallingAppName, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' SET 18/04/2007
            'Developer Guie No 98
            m_lReturn = bPMFunc.getProductOptionValue(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTMultiTreeAccounting, v_vBranch:=m_iSourceID, r_vUnderwriting:=vValue)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getProductOptionValue Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            End If

            'developer Guide No 248
            If Val(vValue) = 1 Then
                bMultiTreeAcc = True
            End If

            'Developer Guie No 98
            m_lReturn = bPMFunc.getProductOptionValue(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTMultiCoWorkManagerTaskRestriction, v_vBranch:=m_iSourceID, r_vUnderwriting:=vValue)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getProductOptionValue (Restricted Client View) Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            'developer Guide No 248
            If Val(vValue) Then
                bRestrictedTaskView = True
            End If

            If bRestrictedTaskView And bMultiTreeAcc Then
                m_bRestrictedTaskView = True
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
                If m_oTaskInstance IsNot Nothing Then
                    m_oTaskInstance.Dispose()
                    m_oTaskInstance = Nothing
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
    ' 1 - Add the Task Instance.
    ' 2 - Add any Keys supplied.
    ' ***************************************************************** '
    'DAK141299
    'developer guide no. 33
    Public Function CreateNew(ByVal v_lPMWrkTaskID As Integer, ByVal v_lPMWrkTaskGroupID As Integer, ByVal v_sCustomer As String, ByVal v_dtTaskDueDate As Date, ByVal v_lPMUserGroupID As Integer, ByVal v_sDescription As String, ByVal v_iTaskStatus As Integer, ByVal v_iIsUrgent As Integer, ByRef r_lPMWrkTaskInstanceCnt As Integer, Optional ByVal v_dtDateCreated As Date = #12/30/1899#, Optional ByVal v_iCreatedByID As Integer = 0, Optional ByVal v_iUserID As Integer = 0, Optional ByVal v_vKeyArray(,) As Object = Nothing, Optional ByVal v_iIsVisible As Integer = gPMConstants.PMEReturnCode.PMTrue) As Integer

        Dim result As Integer = 0
        Dim sKeyName As String = "", sKeyValue As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a New Task Instance Class
            m_lReturn = NewTaskInstance()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the Defaults
            m_oTaskInstance.Default_Renamed()

            ' Set the properties
            With m_oTaskInstance
                .PmwrkTaskGroupID = v_lPMWrkTaskGroupID
                .PmwrkTaskID = v_lPMWrkTaskID
                .Customer = v_sCustomer
                .TaskDueDate = v_dtTaskDueDate
                .PmuserGroupID = v_lPMUserGroupID
                .Description = v_sDescription
                .TaskStatus = v_iTaskStatus
                .IsUrgent = v_iIsUrgent
                If v_iCreatedByID > 0 Then
                    .CreatedByID = v_iCreatedByID
                    .DateCreated = v_dtDateCreated
                End If
                If v_iUserID > 0 Then
                    .UserID = v_iUserID
                End If
                'DAK141299
                .IsVisible = v_iIsVisible
                .SourceID = m_iSourceID
            End With

            m_lReturn = m_oTaskInstance.Add()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Return the Key of the row added.
            r_lPMWrkTaskInstanceCnt = m_oTaskInstance.PMWrkTaskInstanceCnt

            ' Add the Task Instance Keys, if there are any

            If Not Informations.IsNothing(v_vKeyArray) Then
                If Informations.IsArray(v_vKeyArray) Then

                    'DAK240100
                    'developer guide no 18. 
                    For lRow As Integer = v_vKeyArray.GetLowerBound(1) To v_vKeyArray.GetUpperBound(1)

                        sKeyName = CStr(v_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lRow))

                        sKeyValue = CStr(v_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                        m_lReturn = AddTaskInstKey(v_lPMWrkTaskInstanceCnt:=r_lPMWrkTaskInstanceCnt, v_sKeyName:=sKeyName, v_sKeyValue:=sKeyValue)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                    Next lRow

                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateNewFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateNew", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function DeleteTaskTemplate(ByVal PMWrkTaskInstTempCnt As Integer) As Integer
        Dim result As Integer = 0
        Dim sSQL As String = ""

        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            sSQL = "Delete From PMWrk_Task_Instance_Temp WHERE pmwrk_task_instance_temp_cnt = " & PMWrkTaskInstTempCnt

            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="DeleteTaskTemplate", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteTaskTemplate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteTaskTemplate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

            ' Select the Task Instance
            m_lReturn = GetTaskInstance(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Return the Details.
            With m_oTaskInstance
                r_lPMWrkTaskGroupID = .PmwrkTaskGroupID
                r_lPMWrkTaskID = .PmwrkTaskID
                r_sCustomer = .Customer
                r_dtTaskDueDate = .TaskDueDate
                r_lPMUserGroupID = .PmuserGroupID
                r_iUserID = .UserID
                r_sDescription = .Description
                r_iTaskStatus = .TaskStatus
                r_iIsUrgent = .IsUrgent
                r_dtDateCreated = .DateCreated
                r_iCreatedByID = .CreatedByID
                r_dtLastModified = .LastModified
                r_iModifiedByID = .ModifiedByID
                'DAK141299

                If Not Informations.IsNothing(r_vIsVisible) Then

                    .IsVisible = CInt(r_vIsVisible)
                End If
            End With

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

            With m_oDatabase

                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="pmwrk_task_instance_cnt", vValue:=CStr(v_lPMWrkTaskInstanceCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .SQLSelect(sSQL:=ACGetTaskInstKeysSQL, sSQLName:=ACGetTaskInstKeysName, bStoredProcedure:=ACGetTaskInstKeysStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vKeyArray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetTaskInstKeysFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTaskInstKeys", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetTaskDetails
    '
    ' Description: Gets the details for the Task itself.
    '
    ' DAK121099 - Changes for Product Licencing and linked objects
    ' ***************************************************************** '
    Public Function GetTaskDetails(ByVal v_lPMWrkTaskID As Integer, ByRef r_iIsSystemTask As Integer, ByRef r_iTypeOfTask As Integer, ByRef r_lPMNavProcessID As Integer, ByRef r_sComponentObjectName As String, ByRef r_sComponentClassName As String, ByRef r_lAutoDeleteAfterNumDays As Integer, ByRef r_lDisplayIcon As Integer, ByRef r_iIsViewOnlyTask As Integer, ByRef r_sLinkedObjectName As String, ByRef r_sLinkedClassName As String, ByRef r_sLinkedCaption As String, ByRef r_iIsAvailableTask As Integer) As Integer

        Dim result As Integer = 0
        Dim oTask As bPMTask.Business

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create Component Services

            ' Create bPMTask.Business

            oTask = New bPMTask.Business
            m_lReturn = oTask.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                oTask = Nothing
                Return m_lReturn
            End If

            ' Get the Task Details from the DB

            m_lReturn = oTask.GetDetails(v_lPMWrkTaskID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                oTask.Dispose()
                oTask = Nothing
                Return m_lReturn
            End If

            ' Return the Task Details

            m_lReturn = oTask.GetNext(vIsSystemTask:=r_iIsSystemTask, vTypeOfTask:=r_iTypeOfTask, vPMNavProcessId:=r_lPMNavProcessID, vComponentObjectName:=r_sComponentObjectName, vComponentClassName:=r_sComponentClassName, vAutoDeleteAfterNumDays:=r_lAutoDeleteAfterNumDays, vDisplayIcon:=r_lDisplayIcon, vIsViewOnlyTask:=r_iIsViewOnlyTask, vLinkedObjectName:=r_sLinkedObjectName, vLinkedClassName:=r_sLinkedClassName, vLinkedCaption:=r_sLinkedCaption, vIsAvailableTask:=r_iIsAvailableTask)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                oTask.Dispose()
                oTask = Nothing
                Return m_lReturn
            End If

            ' Terminate bPMTask.Business

            oTask.Dispose()
            oTask = Nothing

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
    ' Note:        The User Group/User ID can only be changed via the
    '              assign/reassign methods.
    '              The Task Status can only be changed via the StartTask,
    '              CompleteTask, InCompleteTask methods.
    '              The TaskType cannot be amended.
    'DAK141299
    '              Cannot change IsVisible.
    '
    ' 1 - Cannot change the details if the Task is InProgress, Complete
    ' 2 - Amend the details.
    ' ***************************************************************** '
    Public Function AmendDetails(ByVal v_lPMWrkTaskInstanceCnt As Integer, ByVal v_sCustomer As String, ByVal v_dtTaskDueDate As Date, ByVal v_sDescription As String, ByVal v_iIsUrgent As Integer, Optional ByVal v_dtLastModified As Date = #12/30/1899#, Optional ByVal v_iModifiedByID As Integer = 0) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Select the Task Instance
            m_lReturn = GetTaskInstance(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Amend the Task Instance
            With m_oTaskInstance

                ' If the Task Is Already In Progress
                If .TaskStatus = gPMConstants.PMEWrkManTaskStatus.pmeWMTSInProgress Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Cannot Amend a Task which is Already in Progress PMWrkTaskInstanceCnt = " & v_lPMWrkTaskInstanceCnt, vApp:=ACApp, vClass:=ACClass)
                    Return gPMConstants.PMEReturnCode.PMMAlreadyInUse
                End If

                ' If the Task is Complete
                If .TaskStatus = gPMConstants.PMEWrkManTaskStatus.pmeWMTSComplete Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Cannot Amend a Task which is Already Complete PMWrkTaskInstanceCnt = " & v_lPMWrkTaskInstanceCnt, vApp:=ACApp, vClass:=ACClass)
                    Return gPMConstants.PMEReturnCode.PMInvalidRequest
                End If

                .Customer = v_sCustomer
                .TaskDueDate = v_dtTaskDueDate
                .Description = v_sDescription
                .IsUrgent = v_iIsUrgent
                If v_iModifiedByID > 0 Then
                    .LastModified = v_dtLastModified
                    .ModifiedByID = v_iModifiedByID
                Else
                    .LastModified = DateTime.Now
                    .ModifiedByID = m_iUserID
                End If
            End With

            ' Update the Task Instance
            m_lReturn = m_oTaskInstance.Update()
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

            ' Select the Task Instance
            m_lReturn = GetTaskInstance(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check the we can do this action.
            With m_oTaskInstance
                ' If the Task Is Already In Progress
                If .TaskStatus = gPMConstants.PMEWrkManTaskStatus.pmeWMTSInProgress Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Cannot Assign a Task which is Already in Progress PMWrkTaskInstanceCnt = " & v_lPMWrkTaskInstanceCnt, vApp:=ACApp, vClass:=ACClass)
                    Return gPMConstants.PMEReturnCode.PMMAlreadyInUse
                End If

                ' If the Task is Complete
                If .TaskStatus = gPMConstants.PMEWrkManTaskStatus.pmeWMTSComplete Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Cannot Assign a Task which is Already Complete PMWrkTaskInstanceCnt = " & v_lPMWrkTaskInstanceCnt, vApp:=ACApp, vClass:=ACClass)
                    Return gPMConstants.PMEReturnCode.PMInvalidRequest
                End If

                ' Set the Assigned User ID
                .UserID = v_iUserID

                ' Set the Last Modified Fields
                .ModifiedByID = m_iUserID
                .LastModified = DateTime.Now

            End With

            ' Update the Task Instance
            m_lReturn = m_oTaskInstance.Update()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
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
    ' 1 - Check that the Task is not InProgress or Complete.
    ' 2 - ReAssign the Task
    ' ***************************************************************** '
    Public Function ReAssign(ByVal v_lPMWrkTaskInstanceCnt As Integer, ByVal v_lPMUserGroupID As Integer, Optional ByVal v_iUserID As Integer = 0) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Select the Task Instance
            m_lReturn = GetTaskInstance(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            With m_oTaskInstance

                ' If the Task Is Already In Progress
                If .TaskStatus = gPMConstants.PMEWrkManTaskStatus.pmeWMTSInProgress Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Cannot Assign a Task which is Already in Progress PMWrkTaskInstanceCnt = " & v_lPMWrkTaskInstanceCnt, vApp:=ACApp, vClass:=ACClass)
                    Return gPMConstants.PMEReturnCode.PMMAlreadyInUse
                End If

                ' If the Task is Complete
                If .TaskStatus = gPMConstants.PMEWrkManTaskStatus.pmeWMTSComplete Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Cannot Start a Task which is Already Complete PMWrkTaskInstanceCnt = " & v_lPMWrkTaskInstanceCnt, vApp:=ACApp, vClass:=ACClass)
                    Return gPMConstants.PMEReturnCode.PMInvalidRequest
                End If

                ' Set the Assigned User Group
                .PmuserGroupID = v_lPMUserGroupID
                ' If supplied, set the assigned User
                If v_iUserID > 0 Then
                    .UserID = v_iUserID
                Else

                    .UserID = Nothing
                End If
                ' Set the last Amended Fields
                .ModifiedByID = m_iUserID
                .LastModified = DateTime.Now
            End With

            ' Update the Task Instance
            m_lReturn = m_oTaskInstance.Update()
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
    ' Name: SetStatusComplete
    '
    ' Description: Set the Task Status to Complete if the action
    '              is valid.
    ' ***************************************************************** '
    Public Function SetStatusComplete(ByVal v_lPMWrkTaskInstanceCnt As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Select the Task Instance
            m_lReturn = GetTaskInstance(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check the we can do this action.
            With m_oTaskInstance

                '        ' If the Task is Assigned to Someone Else
                '        If (.UserID <> 0) _
                ''        And (.UserID <> m_iUserID) Then
                '            LogMessage m_sUsername, _
                ''                iType:=PMLogError, _
                ''                sMsg:="Cannot Complete a Task which is Assigned to someone else PMWrkTaskInstanceCnt = " _
                ''                    & v_lPMWrkTaskInstanceCnt, _
                ''                vApp:=ACApp, _
                ''                vClass:=ACClass
                '            SetStatusComplete = PMMNoAccess
                '            Exit Function
                '        End If

                ' Mark it as In Progress
                .TaskStatus = gPMConstants.PMEWrkManTaskStatus.pmeWMTSComplete
                ' Set the Last Modified Fields
                .ModifiedByID = m_iUserID
                .LastModified = DateTime.Now

            End With

            ' Update the Task Instance
            m_lReturn = m_oTaskInstance.Update()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
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
    ' Description: Set the Task Status to Incomplete if this action
    '              is valid.
    '
    ' ***************************************************************** '
    Public Function SetStatusInComplete(ByVal v_lPMWrkTaskInstanceCnt As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Select the Task Instance
            m_lReturn = GetTaskInstance(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check the we can do this action.
            With m_oTaskInstance

                '        ' If the Task is Assigned to Someone Else
                '        If (.UserID <> 0) _
                ''        And (.UserID <> m_iUserID) Then
                '            LogMessage m_sUsername, _
                ''                iType:=PMLogError, _
                ''                sMsg:="Cannot Start a Task which is Assigned to someone else PMWrkTaskInstanceCnt = " _
                ''                    & v_lPMWrkTaskInstanceCnt, _
                ''                vApp:=ACApp, _
                ''                vClass:=ACClass
                '            SetStatusInComplete = PMMNoAccess
                '            Exit Function
                '        End If

                ' Mark it as In Progress
                .TaskStatus = gPMConstants.PMEWrkManTaskStatus.pmeWMTSIncomplete
                ' Set the Last Modified Fields
                .ModifiedByID = m_iUserID
                .LastModified = DateTime.Now

            End With

            ' Update the Task Instance
            m_lReturn = m_oTaskInstance.Update()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
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
    ' Description: Set the Task Status to In Progress if this action
    '              is valid.
    '
    ' DAK121099 - Changes for Product Licencing and linked objects
    ' ***************************************************************** '
    Public Function SetStatusInProgress(ByVal v_lPMWrkTaskInstanceCnt As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Select the Task Instance
            m_lReturn = GetTaskInstance(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check the we can do this action.
            With m_oTaskInstance
                ' If the Task Is Already In Progress
                If .TaskStatus = gPMConstants.PMEWrkManTaskStatus.pmeWMTSInProgress Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Cannot Start a Task which is Already in Progress PMWrkTaskInstanceCnt = " & v_lPMWrkTaskInstanceCnt, vApp:=ACApp, vClass:=ACClass)
                    Return gPMConstants.PMEReturnCode.PMMAlreadyInUse
                End If

                ' If the Task Is Already Complete
                If .TaskStatus = gPMConstants.PMEWrkManTaskStatus.pmeWMTSComplete Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Cannot Start a Task which is Already Complete PMWrkTaskInstanceCnt = " & v_lPMWrkTaskInstanceCnt, vApp:=ACApp, vClass:=ACClass)
                    Return gPMConstants.PMEReturnCode.PMInvalidRequest
                End If

                ' If the licence limit has been exceeded for the category
                m_lReturn = CheckLicenceLimit(v_lPMTaskInstanceCnt:= .PMWrkTaskInstanceCnt, v_lPMTaskID:= .PmwrkTaskID)
                result = m_lReturn
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMWarnLicenceExceeded Then
                    Return result
                End If

                '        ' If the Task is Assigned to Someone Else
                '        If (.UserID <> 0) _
                ''        And (.UserID <> m_iUserID) Then
                '            LogMessage m_sUsername, _
                ''                iType:=PMLogError, _
                ''                sMsg:="Cannot Start a Task which is Assigned to someone else PMWrkTaskInstanceCnt = " _
                ''                    & v_lPMWrkTaskInstanceCnt, _
                ''                vApp:=ACApp, _
                ''                vClass:=ACClass
                '            SetStatusInProgress = PMMNoAccess
                '            Exit Function
                '        End If

                ' Mark it as In Progress
                .TaskStatus = gPMConstants.PMEWrkManTaskStatus.pmeWMTSInProgress
                ' Set the Assigned User to the User who is running it.
                .UserID = m_iUserID
                ' Set the audit fields
                .ModifiedByID = m_iUserID
                .LastModified = DateTime.Now

            End With

            ' Update the Task Instance
            m_lReturn = m_oTaskInstance.Update()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
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

            ' Select the Task Instance
            m_lReturn = GetTaskInstance(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check that we can do this Action
            With m_oTaskInstance
                ' If the Task Is Already In Progress
                If .TaskStatus <> gPMConstants.PMEWrkManTaskStatus.pmeWMTSComplete Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Cannot Delete a Task which is NOT Complete PMWrkTaskInstanceCnt = " & v_lPMWrkTaskInstanceCnt, vApp:=ACApp, vClass:=ACClass)
                    Return gPMConstants.PMEReturnCode.PMInvalidRequest
                End If
            End With

            ' Delete the Task Instance
            m_lReturn = m_oTaskInstance.Delete()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
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

            m_lReturn = m_oDatabase.SQLBeginTrans()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Call the Stored Procedure
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAutoDeleteSQL, sSQLName:=ACAutoDeleteName, bStoredProcedure:=ACAutoDeleteStored)
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = m_oDatabase.SQLCommitTrans()
            Else
                m_lReturn = m_oDatabase.SQLRollbackTrans()
            End If

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
    ' Name: AddTaskID
    '
    ' Description: Adds task template ID to the document template
    ' ***************************************************************** '

    Public Function AddTaskID(ByVal lDocumentTemplateId As String, ByRef lTaskTempID As String) As Integer
        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = "Update Document_Template Set pmwrk_task_instance_temp_cnt = " & lTaskTempID &
                   " Where document_template_id = " & lDocumentTemplateId

            With m_oDatabase
                m_lReturn = .SQLAction(sSQL:=sSQL, sSQLName:="AddTaskID", bStoredProcedure:=False)
            End With

            result = m_lReturn

        Catch
        End Try




        Return result
    End Function


    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: GetTaskInstance
    '
    ' Description: Gets the Task Instance for the Specified Key.
    '              It may already be the one loaded, so check first,
    '              before going to the database.
    ' ***************************************************************** '
    Private Function GetTaskInstance(ByVal v_lPMWrkTaskInstanceCnt As Integer) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        If v_lPMWrkTaskInstanceCnt < 1 Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="PMWrkTaskInstanceCnt must be specified.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTaskInstance")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If v_lPMWrkTaskInstanceCnt = m_oTaskInstance.PMWrkTaskInstanceCnt Then
            ' Already Loaded, Nothing to do.
        Else
            ' Create a New Task Instance
            m_lReturn = NewTaskInstance()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            ' Load the data from the Database
            m_oTaskInstance.PMWrkTaskInstanceCnt = v_lPMWrkTaskInstanceCnt
            m_lReturn = m_oTaskInstance.SelectSingle()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: NewTaskInstance
    '
    ' Description: Creates a New Task Instance
    '
    ' ***************************************************************** '
    Private Function NewTaskInstance() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Terminate the Existing Task Instance
        m_oTaskInstance.Dispose()
        ' Create a new one
        m_oTaskInstance = New bPMWrkTaskInstanceTemp.PMWrkInstance()
        m_lReturn = m_oTaskInstance.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: AddTaskInstKey
    '
    ' Description: Adds a single Task Instance Key
    '
    ' ***************************************************************** '
    Public Function AddTaskInstKey(ByVal v_lPMWrkTaskInstanceCnt As Integer, ByVal v_sKeyName As String, ByVal v_sKeyValue As String) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="pmwrk_task_instance_cnt", vValue:=CStr(v_lPMWrkTaskInstanceCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .Parameters.Add(sName:="key_name", vValue:=v_sKeyName.Trim(), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .Parameters.Add(sName:="key_value", vValue:=v_sKeyValue.Trim(), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .SQLAction(sSQL:=ACAddTaskInstKeySQL, sSQLName:=ACAddTaskInstKeyName, bStoredProcedure:=ACAddTaskInstKeyStored, lRecordsAffected:=lRecordsAffected)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If lRecordsAffected < 1 Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddTaskInstKeyFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddTaskInstKey", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: CheckLicenceLimit
    '
    ' Description:
    '
    ' History: 12/10/1999 DAK - Created.
    ' DAK231299 - Replace Task Group Category with Task Category
    ' ***************************************************************** '
    Private Function CheckLicenceLimit(ByRef v_lPMTaskInstanceCnt As Integer, ByRef v_lPMTaskID As Integer) As Integer

        Dim result As Integer = 0
        Dim oTask As bPMTask.Business
        Dim iIsViewOnlyTask As gPMConstants.PMEReturnCode
        Dim oCategoryLookup As bPMTaskCategory.Lookup
        'DAK231299
        Dim lPMTaskCategoryID As Integer
        Dim oCategoryBusiness As bPMTaskCategory.Business
        Dim lLicenceLimit As Integer
        Dim iIsBlockAboveLicenceLimit As gPMConstants.PMEReturnCode
        Dim iIsWarnAboveLicenceLimit As gPMConstants.PMEReturnCode
        Dim lWarnsSinceLicenceUpgrade, lCategoryTaskCount As Integer




        result = gPMConstants.PMEReturnCode.PMTrue

        ' Create Component Services

        ' Create bPMTask.Business

        oTask = New bPMTask.Business
        m_lReturn = oTask.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            oTask = Nothing
            Return m_lReturn
        End If

        ' Get the Task Details from the DB

        m_lReturn = oTask.GetDetails(v_lPMTaskID)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = m_lReturn

            oTask.Dispose()
            oTask = Nothing
            Return result
        End If

        ' Return the whether the Task is view only or not

        m_lReturn = oTask.GetNext(vIsViewOnlyTask:=iIsViewOnlyTask)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = m_lReturn

            oTask.Dispose()
            oTask = Nothing
            Return result
        End If

        ' Terminate bPMTask.Business

        oTask.Dispose()
        oTask = Nothing

        If iIsViewOnlyTask = gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        ' Create Component Services

        'DAK231299
        ' Create bPMTaskCategory.Lookup

        oCategoryLookup = New bPMTaskCategory.Lookup
        m_lReturn = oCategoryLookup.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = m_lReturn
            oCategoryLookup = Nothing
            Return result
        End If

        ' Get the Category id for this task instance
        'DAK231299

        m_lReturn = oCategoryLookup.GetInstanceCategory(v_lPMTaskInstanceCnt:=v_lPMTaskInstanceCnt, r_lPMTaskCategoryID:=lPMTaskCategoryID)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = m_lReturn

            oCategoryLookup.Dispose()
            oCategoryLookup = Nothing
            Return result
        End If

        ' Terminate bPMTaskGroupCategory.Lookup

        oCategoryLookup.Dispose()
        oCategoryLookup = Nothing

        ' Create Component Services

        'DAK231299
        ' Create bPMTaskCategory.Business

        oCategoryBusiness = New bPMTaskCategory.Business
        m_lReturn = oCategoryBusiness.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = m_lReturn
            oCategoryBusiness = Nothing
            Return result
        End If

        ' Get the category details
        'DAK231299

        m_lReturn = oCategoryBusiness.GetDetails(vPMTaskCategoryId:=lPMTaskCategoryID)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = m_lReturn

            oCategoryBusiness.Dispose()
            oCategoryBusiness = Nothing
            Return result
        End If

        'DAK070100

        oCategoryBusiness.CurrentRecord = 1

        m_lReturn = oCategoryBusiness.ValidateLicenceLimit()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = m_lReturn

            oCategoryBusiness.Dispose()
            oCategoryBusiness = Nothing
            Return result
        End If


        m_lReturn = oCategoryBusiness.GetNext(vLicenceLimit:=lLicenceLimit, vIsBlockAboveLicenceLimit:=iIsBlockAboveLicenceLimit, vIsWarnAboveLicenceLimit:=iIsWarnAboveLicenceLimit, vWarnsSinceLicenceUpgrade:=lWarnsSinceLicenceUpgrade)

        ' Get the number of current licenced tasks in progress for the
        ' category from the DB
        'DAK231299

        m_lReturn = oCategoryBusiness.CountCategoryTasks(v_lPMTaskCategoryID:=lPMTaskCategoryID, r_lCategoryTaskCount:=lCategoryTaskCount)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = m_lReturn

            oCategoryBusiness.Dispose()
            oCategoryBusiness = Nothing
            Return result
        End If

        If lCategoryTaskCount >= lLicenceLimit Then
            If iIsBlockAboveLicenceLimit = gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMBlockLicenceExceeded
            ElseIf iIsWarnAboveLicenceLimit = gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMWarnLicenceExceeded
                ' Increment the number of warnings for the category

                m_lReturn = oCategoryBusiness.EditUpdate(lRow:=1, vWarnsSinceLicenceUpgrade:=(lWarnsSinceLicenceUpgrade + 1))
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn

                    oCategoryBusiness.Dispose()
                    oCategoryBusiness = Nothing
                    Return result
                End If


                m_lReturn = oCategoryBusiness.Update()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn

                    oCategoryBusiness.Dispose()
                    oCategoryBusiness = Nothing
                    Return result
                End If
            End If
        End If


        oCategoryBusiness.Dispose()
        oCategoryBusiness = Nothing

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


    ' ***************************************************************** '
    ' Name          : UpdateDocumentTemplateTaskDetails
    ' Description   : Update the Document Templates' TaskTemplate  Details.
    ' Edit History  :
    ' DJM 19/02/2003 PN10480 : Copied fix from 1.8.5 issue 4697.
    ' ***************************************************************** '
    Public Function UpdateDocumentTemplateTaskDetails(ByVal v_lPMWrkTaskInstanceCnt As Integer, ByVal v_sCustomer As String, ByVal v_lPMWrkTaskGroupID As Integer, ByVal v_lPMWrkTaskID As Integer, ByVal v_sDescription As String, ByVal v_dtTaskDueDate As Date, ByVal v_lPMUserGroupID As Integer, ByVal v_iUserID As Integer, ByVal v_iTaskStatus As Integer, ByVal v_iIsUrgent As Integer, ByVal v_dtDateCreated As Date, ByVal v_iCreatedByID As Integer, ByVal v_dtLastModifiedDate As Date, ByVal v_iLastModifiedByID As Integer, ByVal v_iIsVisible As Integer) As Integer

        Dim result As Integer = 0
        Const ACUpdatePMWrkTaskIntanceTempStored As Boolean = True
        Const ACUpdatePMWrkTaskIntanceTempName As String = "UpdatePMWrkTaskIntanceTemp"
        'developer guide no. 39
        Const ACUpdatePMWrkTaskIntanceTempSQL As String = "spe_PMWrk_Task_Instance_Temp_upd"
        Dim lRecordsAffected As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                ' Clear the Database Parameters Collection
                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="pmwrk_task_instance_temp_cnt", vValue:=CStr(v_lPMWrkTaskInstanceCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If v_sCustomer = "" Then

                    'developer guide no. 85
                    m_lReturn = .Parameters.Add(sName:="customer", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                Else
                    m_lReturn = .Parameters.Add(sName:="customer", vValue:=v_sCustomer, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                End If
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .Parameters.Add(sName:="pmwrk_task_group_id", vValue:=CStr(v_lPMWrkTaskGroupID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .Parameters.Add(sName:="pmwrk_task_id", vValue:=CStr(v_lPMWrkTaskID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If v_sDescription = "" Then

                    'developer guide no. 85
                    m_lReturn = .Parameters.Add(sName:="description", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                Else
                    m_lReturn = .Parameters.Add(sName:="description", vValue:=v_sDescription, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                End If
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .Parameters.Add(sName:="task_due_date", vValue:=v_dtTaskDueDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .Parameters.Add(sName:="pmuser_group_id", vValue:=CStr(v_lPMUserGroupID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                If (v_iUserID < 1) Or (Convert.IsDBNull(v_iUserID) Or Informations.IsNothing(v_iUserID)) Then

                    'developer guide no. 85
                    m_lReturn = .Parameters.Add(sName:="user_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                Else
                    m_lReturn = .Parameters.Add(sName:="user_id", vValue:=CStr(v_iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                End If

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .Parameters.Add(sName:="task_status", vValue:=CStr(v_iTaskStatus), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .Parameters.Add(sName:="is_urgent", vValue:=CStr(v_iIsUrgent), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .Parameters.Add(sName:="date_created", vValue:=v_dtDateCreated, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .Parameters.Add(sName:="created_by_id", vValue:=CStr(v_iCreatedByID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If



                If (v_iLastModifiedByID < 1) Or (Convert.IsDBNull(v_iLastModifiedByID) Or Informations.IsNothing(v_iLastModifiedByID)) Then


                    'developer guide no. 85
                    m_lReturn = .Parameters.Add(sName:="last_modified", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    'developer guide no. 85
                    m_lReturn = .Parameters.Add(sName:="modified_by_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                Else
                    m_lReturn = .Parameters.Add(sName:="last_modified", vValue:=v_dtLastModifiedDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = .Parameters.Add(sName:="modified_by_id", vValue:=CStr(v_iLastModifiedByID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                End If

                m_lReturn = .Parameters.Add(sName:="is_visible", vValue:=CStr(v_iIsVisible), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                ' Execute SQL Statement
                m_lReturn = .SQLAction(sSQL:=ACUpdatePMWrkTaskIntanceTempSQL, sSQLName:=ACUpdatePMWrkTaskIntanceTempName, bStoredProcedure:=ACUpdatePMWrkTaskIntanceTempStored, lRecordsAffected:=lRecordsAffected)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Check to see that the record was updated OK
                If lRecordsAffected > 0 Then
                    ' Updated No action required
                Else
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateDocumentTemplateTaskDetails Failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateDocumentTemplateTaskDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name          : CheckDocumentTemplateTaskDetailsExists
    ' Description   : Check the Document Templates' TaskTemplate  Details Exists
    ' Notes         : Returns
    '                   1. PMNotFound  (if Missing)
    '                   2. PMTrue      (if Found)
    '                   3. PMError     (if error)
    ' Edit History  :
    ' DJM 19/02/2003 PN10480 : Copied fix from 1.8.5 issue 4697.
    ' ***************************************************************** '
    Public Function CheckDocumentTemplateTaskDetailsExists(ByVal v_lPMWrkTaskInstanceCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vArray(,) As Object = Nothing
        Dim lRecordsFound As Integer

        ' Default to PMNotFound
        result = gPMConstants.PMEReturnCode.PMNotFound

        If v_lPMWrkTaskInstanceCnt > 0 Then

            sSQL = "SELECT COUNT(*) as RecordAvailable FROM pmwrk_task_instance_temp where pmwrk_task_instance_temp_cnt = " & v_lPMWrkTaskInstanceCnt

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="CheckDocumentTaskTemplateDetailsExists", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            If Informations.IsArray(vArray) Then

                lRecordsFound = CInt(vArray(0, 0))
                If lRecordsFound > 0 Then
                    result = gPMConstants.PMEReturnCode.PMTrue
                End If
            End If

        End If

        Return result



        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckDocumentTemplateTaskDetailsExists Failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckDocumentTemplateTaskDetailsExists", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

        Return result

    End Function
    Private Shared _DefaultInstance As Business = Nothing
    Public Shared ReadOnly Property DefaultInstance() As Business
        Get
            If _DefaultInstance Is Nothing Then
                _DefaultInstance = New Business
            End If
            Return _DefaultInstance
        End Get
    End Property
End Class
