Option Strict Off
Option Explicit On
'developer guide no 129. 
'Start
Imports SSP.Shared
'End
<System.Runtime.InteropServices.ProgId("FormClass_NET.FormClass")> _
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
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long


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

            m_lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create Task Control
            m_oBusiness = New bPMWrkTaskInstanceTemp.Business()
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
                End If
                m_oBusiness = Nothing
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                End If
                m_oDatabase = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    Public Function GetDefaultTask(ByRef r_lTaskID As Object, ByRef r_lGroupID As Object) As Integer
        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vArray(,) As Object = Nothing

        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            sSQL = "Select Task.PMWrk_Task_ID , GroupLink.PMWrk_Task_Group_ID "
            sSQL = sSQL & "From PMWrk_Task Task, PMWrk_Task_Group_Task GroupLink "
            sSQL = sSQL & "Where Task.Description = 'Retrieve Client' "
            sSQL = sSQL & "And Task.PMWrk_Task_ID = GroupLink.PMWrk_Task_ID "

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetDefaultTask", bStoredProcedure:=False, lNumberRecords:=0, vResultArray:=vArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsArray(vArray) Then


                r_lTaskID = vArray(0, 0)


                r_lGroupID = vArray(1, 0)
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDefaultTask Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDefaultTask", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CreateKeys
    '
    ' Description: Creates the Keys associated with tasks.
    '              MS030701
    '
    ' ***************************************************************** '

    Public Function CreateKeys(ByVal v_lTaskInstanceID As Integer, ByVal v_lPartyCnt As Integer, ByVal v_sShortName As String, ByVal v_sPartyType As String, ByVal v_sResolvedName As String, ByVal v_lInsuranceFileCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer) As Integer



        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Try
            ' We need to store this information in the architecture database
            ' so that when we run the task, we have enough information to
            ' open Client Manager to the right client and hopefully policy

            With m_oDatabase

                m_lReturn = m_oBusiness.AddTaskInstKey(v_lPMWrkTaskInstanceCnt:=v_lTaskInstanceID, v_sKeyName:="party_cnt", v_sKeyValue:=CStr(v_lPartyCnt))


                m_lReturn = m_oBusiness.AddTaskInstKey(v_lPMWrkTaskInstanceCnt:=v_lTaskInstanceID, v_sKeyName:="party_type", v_sKeyValue:=v_sPartyType)


                m_lReturn = m_oBusiness.AddTaskInstKey(v_lPMWrkTaskInstanceCnt:=v_lTaskInstanceID, v_sKeyName:="shortname", v_sKeyValue:=v_sShortName)


                m_lReturn = m_oBusiness.AddTaskInstKey(v_lPMWrkTaskInstanceCnt:=v_lTaskInstanceID, v_sKeyName:="resolved_name", v_sKeyValue:=v_sResolvedName)


                ' Check if we have a policy
                If v_lInsuranceFileCnt > 0 Then

                    m_lReturn = m_oBusiness.AddTaskInstKey(v_lPMWrkTaskInstanceCnt:=v_lTaskInstanceID, v_sKeyName:="insurance_file_cnt", v_sKeyValue:=CStr(v_lInsuranceFileCnt))

                End If

                ' Check if we have a policy
                If v_lInsuranceFolderCnt > 0 Then

                    m_lReturn = m_oBusiness.AddTaskInstKey(v_lPMWrkTaskInstanceCnt:=v_lTaskInstanceID, v_sKeyName:="insurance_folder_cnt", v_sKeyValue:=CStr(v_lInsuranceFolderCnt))

                End If

            End With


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateKeys", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function DeleteTemplate(ByVal PMWrkTaskInstTempCnt As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_oBusiness.DeleteTaskTemplate(PMWrkTaskInstTempCnt:=PMWrkTaskInstTempCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteTemplate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteTemplate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CreateNew
    '
    ' Description: Creates a New Task Instance.
    '
    ' ***************************************************************** '
    'DAK141299
    Public Function CreateNew(ByVal v_lPMWrkTaskGroupID As Integer, ByVal v_lPMWrkTaskID As Integer, ByVal v_sCustomer As String, ByVal v_dtTaskDueDate As Date, ByVal v_lPMUserGroupID As Integer, ByVal v_sDescription As String, ByVal v_iTaskStatus As Integer, ByVal v_iIsUrgent As Integer, ByVal v_dtDateCreated As Date, ByVal v_iCreatedByID As Integer, ByRef r_lPMWrkTaskInstanceCnt As Integer, Optional ByVal v_iUserID As Integer = 0, Optional ByVal v_vKeyArray(,) As Object = Nothing, Optional ByVal v_iIsVisible As Integer = gPMConstants.PMEReturnCode.PMTrue, Optional ByVal v_bIsNewInstance As Boolean = False) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a New Task Instance Class
            'DAK141299
            g_bInstance = v_bIsNewInstance


            m_lReturn = m_oBusiness.CreateNew(v_lPMWrkTaskGroupID:=v_lPMWrkTaskGroupID, v_lPMWrkTaskID:=v_lPMWrkTaskID, v_sCustomer:=v_sCustomer, v_dtTaskDueDate:=v_dtTaskDueDate, v_lPMUserGroupID:=v_lPMUserGroupID, v_sDescription:=v_sDescription, v_iTaskStatus:=v_iTaskStatus, v_iIsUrgent:=v_iIsUrgent, r_lPMWrkTaskInstanceCnt:=r_lPMWrkTaskInstanceCnt, v_dtDateCreated:=v_dtDateCreated, v_iCreatedByID:=v_iCreatedByID, v_iUserID:=v_iUserID, v_vKeyArray:=v_vKeyArray, v_iIsVisible:=v_iIsVisible)

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
    ' Name: GetDiaryDays
    '
    ' Edit History
    ' Created 14/3/2006 2006 Diary Tasks
    ' PN27272
    ' ***************************************************************** '
    Public Function GetDiaryDays(ByRef vDiaryDays(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            ' get the user Defined dairy days
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetDiaryDaysSQL, sSQLName:=ACGetDiaryDaysName, bStoredProcedure:=ACGetDiaryDaysStored, vResultArray:=vDiaryDays)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' SQL failed
                Return gPMConstants.PMEReturnCode.PMFalse
            End If




            Return result

        Catch



            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function


    ' ***************************************************************** '
    ' Name: GetDetails
    '
    ' Description: Gets the details for the Task Instance.
    ' ***************************************************************** '
    Public Function GetDetails(ByVal v_lPMWrkTaskInstanceCnt As Integer, ByRef r_lPMWrkTaskGroupID As Integer, ByRef r_lPMWrkTaskID As Integer, ByRef r_sCustomer As String, ByRef r_dtTaskDueDate As Date, ByRef r_lPMUserGroupID As Integer, ByRef r_iUserID As Integer, ByRef r_sDescription As String, ByRef r_iTaskStatus As Integer, ByRef r_iIsUrgent As Integer, ByRef r_dtDateCreated As Date, ByRef r_iCreatedByID As Integer, ByRef r_dtLastModified As Date, ByRef r_iModifiedByID As Integer, Optional ByRef r_vIsVisible As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_oBusiness.GetDetails(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt, r_lPMWrkTaskGroupID:=r_lPMWrkTaskGroupID, r_lPMWrkTaskID:=r_lPMWrkTaskID, r_sCustomer:=r_sCustomer, r_dtTaskDueDate:=r_dtTaskDueDate, r_lPMUserGroupID:=r_lPMUserGroupID, r_iUserID:=r_iUserID, r_sDescription:=r_sDescription, r_iTaskStatus:=r_iTaskStatus, r_iIsUrgent:=r_iIsUrgent, r_dtDateCreated:=r_dtDateCreated, r_iCreatedByID:=r_iCreatedByID, r_dtLastModified:=r_dtLastModified, r_iModifiedByID:=r_iModifiedByID, r_vIsVisible:=r_vIsVisible)

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

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_oBusiness.GetTaskDetails(v_lPMWrkTaskID:=v_lPMWrkTaskID, r_iIsSystemTask:=r_iIsSystemTask, r_iTypeOfTask:=r_iTypeOfTask, r_lPMNavProcessID:=r_lPMNavProcessID, r_sComponentObjectName:=r_sComponentObjectName, r_sComponentClassName:=r_sComponentClassName, r_lAutoDeleteAfterNumDays:=r_lAutoDeleteAfterNumDays, r_lDisplayIcon:=r_lDisplayIcon, r_iIsViewOnlyTask:=r_iIsViewOnlyTask, r_sLinkedObjectName:=r_sLinkedObjectName, r_sLinkedClassName:=r_sLinkedClassName, r_sLinkedCaption:=r_sLinkedCaption, r_iIsAvailableTask:=r_iIsAvailableTask)


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
    ' ***************************************************************** '
    Public Function AmendDetails(ByVal v_lPMWrkTaskInstanceCnt As Integer, ByVal v_sCustomer As String, ByVal v_dtTaskDueDate As Date, ByVal v_sDescription As String, ByVal v_iIsUrgent As Integer, ByVal v_dtLastModified As Date, ByVal v_iModifiedByID As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_oBusiness.AmendDetails(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt, v_sCustomer:=v_sCustomer, v_dtTaskDueDate:=v_dtTaskDueDate, v_sDescription:=v_sDescription, v_iIsUrgent:=v_iIsUrgent, v_dtLastModified:=v_dtLastModified, v_iModifiedByID:=v_iModifiedByID)
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
    Public Function ReAssign(ByVal v_lPMWrkTaskInstanceCnt As Integer, ByVal v_lPMUserGroupID As Integer, Optional ByVal v_iUserID As Integer = 0) As Integer

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
    ' Name          : UpdateDocumentTemplateTaskDetails
    ' Description   : Update the Document Templates' TaskTemplate  Details.
    ' Edit History  :
    ' DJM 19/02/2003 PN10480 : Copied fix from 1.8.5 issue 4697.
    ' ***************************************************************** '
    Public Function UpdateDocumentTemplateTaskDetails(ByVal v_lPMWrkTaskInstanceCnt As Integer, ByVal v_sCustomer As String, ByVal v_lPMWrkTaskGroupID As Integer, ByVal v_lPMWrkTaskID As Integer, ByVal v_sDescription As String, ByVal v_dtTaskDueDate As Date, ByVal v_lPMUserGroupID As Integer, ByVal v_iUserID As Integer, ByVal v_iTaskStatus As Integer, ByVal v_iIsUrgent As Integer, ByVal v_dtDateCreated As Date, ByVal v_iCreatedByID As Integer, ByVal v_dtLastModifiedDate As Date, ByVal v_iLastModifiedByID As Integer, ByVal v_iIsVisible As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_oBusiness.UpdateDocumentTemplateTaskDetails(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt, v_sCustomer:=v_sCustomer, v_lPMWrkTaskGroupID:=v_lPMWrkTaskGroupID, v_lPMWrkTaskID:=v_lPMWrkTaskID, v_sDescription:=v_sDescription, v_dtTaskDueDate:=v_dtTaskDueDate, v_lPMUserGroupID:=v_lPMUserGroupID, v_iUserID:=v_iUserID, v_iTaskStatus:=v_iTaskStatus, v_iIsUrgent:=v_iIsUrgent, v_dtDateCreated:=v_dtDateCreated, v_iCreatedByID:=v_iCreatedByID, v_dtLastModifiedDate:=v_dtLastModifiedDate, v_iLastModifiedByID:=v_iLastModifiedByID, v_iIsVisible:=v_iIsVisible)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

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
        Try


            Return m_oBusiness.CheckDocumentTemplateTaskDetailsExists(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckDocumentTemplateTaskDetailsExists Failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckDocumentTemplateTaskDetailsExists", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
End Class