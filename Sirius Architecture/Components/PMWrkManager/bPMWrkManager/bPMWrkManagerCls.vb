Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Text
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("FormClass_NET.FormClass")> _
Public NotInheritable Class FormClass
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness
    ' ***************************************************************** '
    ' Class Name: FormBusiness
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
    Private Const ACClass As String = "FormBusiness"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Error Code (Private)
    Private m_lReturn As Integer

    Private m_oTaskControl As bPMWrkTaskInstance.TaskControl
    'Private m_oTaskControl As bPMWrkTaskInstance.TaskControl

    Private m_oLock As bPMLock.User
    'Private m_oLock As bPMLock.User

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
#Region "Public functions"

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageId As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long Implements SSP.S4I.Interfaces.IBusiness.Initialise

        ' RDC 13062002 CompServ replaced with BAS module
        'Dim oCS As sPMServerCS.PMServerBusinessCS
        Dim result As Integer = 0
        Dim bMultiTreeAcc, bRestrictedTaskView As Boolean
        Dim vValue As Byte

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            ' Set Username and Password
            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageId
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
            '    Set oCS = New sPMServerCS.PMServerBusinessCS
            '    m_lReturn = oCS.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, _
            'v_lPMProductFamily:=PMProductFamily, _
            'r_bNewInstanceCreated:=m_bCloseDatabase, _
            'r_oCheckedDatabase:=m_oDatabase, _
            'v_vDatabase:=vDatabase)

            m_lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                '        Set oCS = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create Task Control Business
            '    m_lReturn = oCS.CreateBusinessObject( _
            'r_oObject:=m_oTaskControl, _
            'v_sClassName:="bPMWrkTaskInstance.TaskControl", _
            'v_sCallingAppName:=ACApp, _
            'v_sUserName:=m_sUsername, _
            'v_sPassword:=m_sPassword, _
            'v_iUserID:=m_iUserID, _
            'v_iSourceID:=m_iSourceID, _
            'v_iLanguageId:=m_iLanguageID, _
            'v_iCurrencyID:=m_iCurrencyID, _
            'v_iLogLevel:=m_iLogLevel, _
            'v_oDatabase:=m_oDatabase)

            m_oTaskControl = New bPMWrkTaskInstance.TaskControl
            m_lReturn = m_oTaskControl.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                '        Set oCS = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create Lock Business
            '    m_lReturn = oCS.CreateBusinessObject( _
            'r_oObject:=m_oLock, _
            'v_sClassName:="bPMLock.User", _
            'v_sCallingAppName:=ACApp, _
            'v_sUserName:=m_sUsername, _
            'v_sPassword:=m_sPassword, _
            'v_iUserID:=m_iUserID, _
            'v_iSourceID:=m_iSourceID, _
            'v_iLanguageId:=m_iLanguageID, _
            'v_iCurrencyID:=m_iCurrencyID, _
            'v_iLogLevel:=m_iLogLevel, _
            'v_oDatabase:=m_oDatabase)

            m_oLock = New bPMLock.User
            m_lReturn = m_oLock.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                '        Set oCS = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '    Set oCS = Nothing
            ' SET 18/04/2007
            m_lReturn = bPMFunc.getProductOptionValue(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTMultiTreeAccounting, v_vBranch:=m_iSourceID, r_vUnderwriting:=CStr(vValue))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getProductOptionValue Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            End If

            If vValue = 1 Then
                bMultiTreeAcc = True
            End If

            m_lReturn = bPMFunc.getProductOptionValue(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTMultiCoWorkManagerTaskRestriction, v_vBranch:=m_iSourceID, r_vUnderwriting:=CStr(vValue))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getProductOptionValue (Restricted Client View) Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            If vValue = 1 Then
                bRestrictedTaskView = True
            End If

            If bRestrictedTaskView And bMultiTreeAcc Then
                m_bRestrictedTaskView = True
            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
#End Region

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
                If m_oTaskControl IsNot Nothing Then
                    m_oTaskControl.Dispose()
                    m_oTaskControl = Nothing
                End If
                If m_oLock IsNot Nothing Then
                    m_oLock.Dispose()
                    m_oLock = Nothing
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
    ' Name: GetScheduledTasks
    '
    ' Description: Gets the List of Scheduled Tasks, dependant on the
    '              parameters supplied.
    '
    ' ***************************************************************** '
    'developer guide no. 17
    Public Function GetScheduledTasks(ByRef r_vScheduledTaskArray(,) As Object, Optional ByVal v_lTaskStatus As Object = Nothing, Optional ByVal v_lPmuserGroupID As Object = Nothing, Optional ByVal v_iUserID As Object = Nothing, Optional ByVal v_dtDueDateLimit As Object = #12/29/1899#, Optional ByVal v_bOmitCompleted As Object = Nothing, Optional ByVal vGroups As Object = Nothing, Optional ByVal PartyKey As Object = Nothing) As Integer
        Dim result As Integer = 0
        Dim sUserGroups As New StringBuilder

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            bPMAddParameter.AddParameterLite(m_oDatabase, "user_id", m_iUserID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
            If v_lTaskStatus > -1 Then
                bPMAddParameter.AddParameterLite(m_oDatabase, "task_status", v_lTaskStatus, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                bPMAddParameter.AddParameterLite(m_oDatabase, "omit_task_status", False, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMBoolean)
            Else
                If v_bOmitCompleted Then
                    bPMAddParameter.AddParameterLite(m_oDatabase, "task_status", gPMConstants.PMEWrkManTaskStatus.pmeWMTSComplete, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                    bPMAddParameter.AddParameterLite(m_oDatabase, "omit_task_status", True, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMBoolean)
                Else

                    bPMAddParameter.AddParameterLite(m_oDatabase, "task_status", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                    bPMAddParameter.AddParameterLite(m_oDatabase, "omit_task_status", False, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMBoolean)
                End If
            End If

            If v_lPmuserGroupID > 0 Then
                bPMAddParameter.AddParameterLite(m_oDatabase, "pmuser_group_id", v_lPmuserGroupID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            Else

                bPMAddParameter.AddParameterLite(m_oDatabase, "pmuser_group_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            End If

            If v_iUserID > 0 Then
                bPMAddParameter.AddParameterLite(m_oDatabase, "selected_user_id", v_iUserID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            Else

                bPMAddParameter.AddParameterLite(m_oDatabase, "selected_user_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            End If
            'If m_iUserID > 0 Then
            '    bPMAddParameter.AddParameterLite(m_oDatabase, "selected_user_id", m_iUserID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            'Else

            '    bPMAddParameter.AddParameterLite(m_oDatabase, "selected_user_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            'End If


            If v_dtDueDateLimit > #12/30/1899# Then
                bPMAddParameter.AddParameterLite(m_oDatabase, "task_due_date_limit", v_dtDueDateLimit, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)
            Else

                bPMAddParameter.AddParameterLite(m_oDatabase, "task_due_date_limit", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)
            End If

            sUserGroups = New StringBuilder("")
            If Information.IsArray(vGroups) And v_lPmuserGroupID = -1 Then
                For lLoop As Integer = vGroups.GetLowerBound(1) To vGroups.GetUpperBound(1)
                    sUserGroups.Append(vGroups(0, lLoop) & "|")
                Next
            End If
            If sUserGroups.ToString() = "" Then
                bPMAddParameter.AddParameterLite(m_oDatabase, "pmuser_group_string", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            Else
                bPMAddParameter.AddParameterLite(m_oDatabase, "pmuser_group_string", sUserGroups.ToString(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            End If


            bPMAddParameter.AddParameterLite(m_oDatabase, "is_system_task", False, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMBoolean)

            If m_bRestrictedTaskView Then
                bPMAddParameter.AddParameterLite(m_oDatabase, "source_id", m_iSourceID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                bPMAddParameter.AddParameterLite(m_oDatabase, "RestrictTaskList", 1, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            End If
            bPMAddParameter.AddParameterLite(m_oDatabase, "PartyCnt", PartyKey, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetSchedTasksSQL, sSQLName:=ACGetSchedTasksName, bStoredProcedure:=ACGetSchedTasksStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vScheduledTaskArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                r_vScheduledTaskArray = Nothing
                Return result
            End If

            If Not Information.IsArray(r_vScheduledTaskArray) Then
                r_vScheduledTaskArray = Nothing
                Return result
            End If

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetScheduledTasksFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetScheduledTasks", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try
        Return result
    End Function
    ''' <summary>
    ''' Get Batch Tasks List
    ''' </summary>
    ''' <param name="r_dtBatchTask"></param>
    ''' <param name="sTaskStatus"></param>
    ''' <param name="v_dtDueDateLimit"></param>
    ''' <returns></returns>
    Public Function GetBatchTasks(ByRef r_dtBatchTask As DataTable, ByVal sTaskStatus As String, ByVal v_dtDueDateLimit As Object) As Integer
        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMTrue
        Try

            m_oDatabase.Parameters.DeleteAll()
            bPMAddParameter.AddParameterLite(m_oDatabase, "Batch_Status", sTaskStatus, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

            If v_dtDueDateLimit > #12/30/1899# Then
                bPMAddParameter.AddParameterLite(m_oDatabase, "Batch_date_limit", v_dtDueDateLimit, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)
            Else

                bPMAddParameter.AddParameterLite(m_oDatabase, "Batch_date_limit", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)
            End If

            m_lReturn = m_oDatabase.ExecuteDataTable(sSQL:=kACGetBatchTasksSQL, sSQLName:=kACGetBatchTasksName, bStoredProcedure:=kACGetBatchTasksStored, oRecordset:=r_dtBatchTask)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                r_dtBatchTask = Nothing
                Return nResult
            End If

        Catch excep As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetBatchTasksFailed",
                               vApp:=ACApp, vClass:=ACClass, vMethod:="GetBatchTasks", excep:=excep)
        End Try

        Return nResult
    End Function

    ' ***************************************************************** '
    ' Name: GetScheduledSystemTasks
    '
    ' Description: Gets the List of Scheduled System Tasks, dependant
    '               on the parameters supplied.
    '
    ' ***************************************************************** '
    'developer guide no. 17
    Public Function GetScheduledSystemTasks(ByRef r_vSystemTaskArray(,) As Object, Optional ByVal v_dtDueDateLimit As Date = #12/29/1899#, Optional ByVal PartyKey As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            bPMAddParameter.AddParameterLite(m_oDatabase, "user_id", DBNull.Value.ToString(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

            bPMAddParameter.AddParameterLite(m_oDatabase, "task_status", DBNull.Value.ToString(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            bPMAddParameter.AddParameterLite(m_oDatabase, "omit_task_status", DBNull.Value.ToString(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMBoolean)

            bPMAddParameter.AddParameterLite(m_oDatabase, "pmuser_group_id", DBNull.Value.ToString(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            bPMAddParameter.AddParameterLite(m_oDatabase, "selected_user_id", DBNull.Value.ToString(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            If v_dtDueDateLimit > #12/30/1899# Then
                bPMAddParameter.AddParameterLite(m_oDatabase, "task_due_date_limit", v_dtDueDateLimit, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)
            Else

                bPMAddParameter.AddParameterLite(m_oDatabase, "task_due_date_limit", DBNull.Value.ToString(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)
            End If

            bPMAddParameter.AddParameterLite(m_oDatabase, "pmuser_group_string", DBNull.Value.ToString(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "is_system_task", True, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMBoolean)

            bPMAddParameter.AddParameterLite(m_oDatabase, "source_id", DBNull.Value.ToString(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

            bPMAddParameter.AddParameterLite(m_oDatabase, "RestrictTaskList", DBNull.Value.ToString(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            bPMAddParameter.AddParameterLite(m_oDatabase, "PartyCnt", PartyKey, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetSchedTasksSQL, sSQLName:=ACGetSchedTasksName, bStoredProcedure:=ACGetSchedTasksStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vSystemTaskArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                r_vSystemTaskArray = Nothing
                Return result
            End If

            If Not Information.IsArray(r_vSystemTaskArray) Then
                r_vSystemTaskArray = Nothing
                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetScheduledSystemTasksFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetScheduledSystemTasks", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetTaskInstByKey
    '
    ' Description: Gets the task instance detials for a given key
    '
    ' History: 09/05/2000 DAK - Created.
    '
    ' ***************************************************************** '
    Public Function GetTaskInstByKey(ByVal v_sKeyName As String, ByVal v_sKeyValue As String, ByRef r_vTaskArray As String, Optional ByVal v_lTaskStatus As Integer = -1, Optional ByVal v_lPmuserGroupID As Integer = -1, Optional ByVal v_iUserID As Integer = -1, Optional ByVal v_dtDueDateLimit As Date = #12/29/1899#, Optional ByVal v_bOmitCompleted As Boolean = False) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Parameters
            m_oDatabase.Parameters.Clear()

            ' Build SQL
            sSQL = ACGetSchedTasksSelectSQL & _
                   ACGetTasksByKeyFromSQL & _
                   ACgetTasksByKeyWhereSQL

            m_lReturn = m_oDatabase.Parameters.Add(sName:="key_name", vValue:=v_sKeyName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="key_value", vValue:=v_sKeyValue, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Has a Task Status been specified
            If v_lTaskStatus > -1 Then

                ' Yes
                ' Add SQL
                sSQL = sSQL & ACGetSchedTasksStatusSQL
                ' Add Parameter
                m_lReturn = m_oDatabase.Parameters.Add(sName:="task_status", vValue:=CStr(v_lTaskStatus), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Else

                ' No se we are getting All Task Status

                ' Are we Omitting any Task Statis
                If v_bOmitCompleted Then
                    ' Add SQL
                    sSQL = sSQL & ACGetSchedTasksOmitSQL
                    ' Add Parameter
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="task_status", vValue:=CStr(gPMConstants.PMEWrkManTaskStatus.pmeWMTSComplete), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
            End If

            ' Are we looking for Specific User Group Tasks
            If v_lPmuserGroupID > 0 Then
                ' Yes, so add SQL
                sSQL = sSQL & ACGetSchedTasksGroupSQL
                ' Add Parameter
                m_lReturn = m_oDatabase.Parameters.Add(sName:="pmuser_group_id", vValue:=CStr(v_lPmuserGroupID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            ' Are we looking for a Specific Users Tasks
            If v_iUserID > 0 Then
                sSQL = sSQL & ACGetSchedTasksUserSQL
                ' Add Parameter
                m_lReturn = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=CStr(v_iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' Do we have a Due Date Limit
            If v_dtDueDateLimit > #12/30/1899# Then
                sSQL = sSQL & ACGetSchedTasksLimitDateSQL
                ' Add Parameter
                'developer guide no. 40
                m_lReturn = m_oDatabase.Parameters.Add(sName:="due_date_limit", vValue:=v_dtDueDateLimit, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' Always Omit System Tasks
            sSQL = sSQL & ACGetSchedTasksOmitSystemSQL

            ' Add Order By
            sSQL = sSQL & ACGetSchedTasksOrderBySQL

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:=ACGetSchedTasksName, bStoredProcedure:=ACGetSchedTasksStored, lnumberrecords:=gPMConstants.PMAllRecords, vResultArray:=r_vTaskArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                r_vTaskArray = ""
                Return result
            End If

            If Not Information.IsArray(r_vTaskArray) Then
                r_vTaskArray = ""
                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetTaskInstByKey Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTaskInstByKey", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetAvailableTasks
    '
    ' Description: Gets the Tasks that this user is allowed to do.
    '
    '
    ' ***************************************************************** '
    'developer guide no. 17
    Public Function GetAvailableTasks(ByRef r_vAvailableTasksArray(,) As Object) As Integer

        'Start (Girija chokkalingam) - (Tech Spec - S4IRD001 - US Localisation.doc) - (5.3.1.1)
        Dim result As Integer = 0
        Dim iLanguageId As Integer
        'End (Girija chokkalingam) - (Tech Spec - S4IRD001 - US Localisation.doc) - (5.3.1.1)
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Start (Girija chokkalingam) - (Tech Spec - S4IRD001 - US Localisation.doc) - (5.3.1.1)
            m_lReturn = gPMFunctions.GetUserIsAmericanLanguageID(iLanguageId)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'End (Girija chokkalingam) - (Tech Spec - S4IRD001 - US Localisation.doc) - (5.3.1.1)

            With m_oDatabase
                .Parameters.Clear()

                ' Add UserID Parameter
                m_lReturn = .Parameters.Add(sName:="user_id", vValue:=CStr(m_iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Add EffectiveDate Parameter
                'developer guide no. 40
                m_lReturn = .Parameters.Add(sName:="effective_date", vValue:=DateTime.Now, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Add Language ID Parameter
                'Start (Girija chokkalingam) - (Tech Spec - S4IRD001 - US Localisation.doc) - (5.3.1.1)
                'Modified for including the language id
                m_lReturn = .Parameters.Add(sName:="language_id", vValue:=CStr(iLanguageId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                'End (Girija chokkalingam) - (Tech Spec - S4IRD001 - US Localisation.doc) - (5.3.1.1)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAvailTasksSQL, sSQLName:=ACGetAvailTasksName, bStoredProcedure:=ACGetAvailTasksStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vAvailableTasksArray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    r_vAvailableTasksArray = Nothing
                    Return result
                End If

                If Not Information.IsArray(r_vAvailableTasksArray) Then
                    r_vAvailableTasksArray = Nothing
                    Return result
                End If

            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAvailableTasksFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAvailableTasks", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    Public Function GetDetails(ByVal v_lPMWrkTaskInstanceCnt As Integer, ByRef r_lPMWrkTaskGroupID As Integer, ByRef r_lPMWrkTaskID As Integer, ByRef r_sCustomer As String, ByRef r_dtTaskDueDate As Date, ByRef r_lPMUserGroupID As Integer, ByRef r_iUserID As Integer, ByRef r_sDescription As String, ByRef r_iTaskStatus As Integer, ByRef r_iIsUrgent As Integer, ByRef r_dtDateCreated As Date, ByRef r_iCreatedByID As Integer, ByRef r_dtLastModified As Date, ByRef r_iModifiedByID As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = m_oTaskControl.GetDetails(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt, r_lPMWrkTaskGroupID:=r_lPMWrkTaskGroupID, r_lPMWrkTaskID:=r_lPMWrkTaskID, r_sCustomer:=r_sCustomer, r_dtTaskDueDate:=r_dtTaskDueDate, r_lPMUserGroupID:=r_lPMUserGroupID, r_iUserID:=r_iUserID, r_sDescription:=r_sDescription, r_iTaskStatus:=r_iTaskStatus, r_iIsUrgent:=r_iIsUrgent, r_dtDateCreated:=r_dtDateCreated, r_iCreatedByID:=r_iCreatedByID, r_dtLastModified:=r_dtLastModified, r_iModifiedByID:=r_iModifiedByID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDetailsFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetTaskInstKeys
    '
    ' Description: Gets all of the Keys for a Single Task Instance.
    '
    ' ***************************************************************** '
    Public Function GetTaskInstKeys(ByVal v_lPMWrkTaskInstanceCnt As Integer, ByRef r_vKeyArray As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = m_oTaskControl.GetTaskInstKeys(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt, r_vKeyArray:=r_vKeyArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetTaskInstKeysFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTaskInstKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetUserAuthority
    '
    ' Description: Gets the User Groups that the current user is a
    '              Supervisor of.
    '
    ' ***************************************************************** '
    Public Function GetUserAuthority(ByRef r_bIsAdministrator As Boolean, ByRef r_vSupervisedGroups As Object) As Integer

        Dim result As Integer = 0
        Dim oUserGroup As bPMUserGroup.Utilities
        ' RDC 13062002 CompServ replaced with BAS module
        'Dim oCS As sPMServerCS.PMServerBusinessCS

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create the User Group Component to do this work.
            '    Set oCS = New sPMServerCS.PMServerBusinessCS
            '    m_lReturn = oCS.CreateBusinessObject( _
            'r_oObject:=oUserGroup, _
            'v_sClassName:="bPMUserGroup.Utilities", _
            'v_sCallingAppName:=m_sCallingAppName, _
            'v_sUserName:=m_sUsername, _
            'v_sPassword:=m_sPassword, _
            'v_iUserID:=m_iUserID, _
            'v_iSourceID:=m_iSourceID, _
            'v_iLanguageId:=m_iLanguageID, _
            'v_iCurrencyID:=m_iCurrencyID, _
            'v_iLogLevel:=m_iLogLevel, _
            'v_oDatabase:=m_oDatabase)

            oUserGroup = New bPMUserGroup.Utilities
            m_lReturn = oUserGroup.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)

            '    Set oCS = Nothing
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Is the User an Administrator

            m_lReturn = oUserGroup.IsUserAdministrator(v_iUserID:=m_iUserID, r_bIsAdministrator:=r_bIsAdministrator)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn

                oUserGroup.Dispose()
                oUserGroup = Nothing
                Return result
            End If

            ' Get the Groups they Supervise

            m_lReturn = oUserGroup.GetGroupsSupervisedByUser(v_iUserID:=m_iUserID, r_vSupervisedGroups:=r_vSupervisedGroups)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn

                oUserGroup.Dispose()
                oUserGroup = Nothing
                Return result
            End If

            ' Terminate

            oUserGroup.Dispose()
            oUserGroup = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetUserAuthorityFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUserAuthority", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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


            m_lReturn = m_oTaskControl.Assign(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt, v_iUserID:=v_iUserID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AssignFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="Assign", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    Public Function ReAssign(ByVal v_lPMWrkTaskInstanceCnt As Integer, ByVal v_lPmuserGroupID As Integer, Optional ByVal v_iUserID As Integer = 0) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = m_oTaskControl.ReAssign(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt, v_lPmuserGroupID:=v_lPmuserGroupID, v_iUserID:=v_iUserID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ReAssignFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="ReAssign", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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


            m_lReturn = m_oTaskControl.SetStatusComplete(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetStatusCompleteFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetStatusComplete", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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


            m_lReturn = m_oTaskControl.SetStatusInComplete(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetStatusInCompleteFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetStatusInComplete", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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


            m_lReturn = m_oTaskControl.SetStatusInProgress(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetStatusInProgressFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetStatusInProgress", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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


            m_lReturn = m_oTaskControl.Delete(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="Delete", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: AddQuickStartTask
    '
    ' Description: Adds a single Quick Start Task for the Current user.
    '
    ' ***************************************************************** '
    Public Function AddQuickStartTask(ByVal v_lPMWrkTaskGroupID As Integer, ByVal v_lPMWrkTaskID As Integer, ByVal v_lDisplaySequenceNum As Integer) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' CTAF 030101 - Rearranged order of parameters to be the same as the
            '               stored procedure

            With m_oDatabase

                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="pmwrk_task_group_id", vValue:=CStr(v_lPMWrkTaskGroupID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .Parameters.Add(sName:="pmwrk_task_id", vValue:=CStr(v_lPMWrkTaskID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .Parameters.Add(sName:="user_id", vValue:=CStr(m_iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .Parameters.Add(sName:="display_sequence_num", vValue:=CStr(v_lDisplaySequenceNum), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .SQLAction(sSQL:=ACAddQuickStartTaskSQL, sSQLName:=ACAddQuickStartTaskName, bStoredProcedure:=ACAddQuickStartTaskStored, lRecordsAffected:=lRecordsAffected)
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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddQuickStartTaskFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddQuickStartTask", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeleteQuickStartTasks
    '
    ' Description: Deletes All Quick Start Tasks for the current user.
    '
    ' ***************************************************************** '
    Public Function DeleteQuickStartTasks() As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="user_id", vValue:=CStr(m_iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .SQLAction(sSQL:=ACDelQuickStartTasksSQL, sSQLName:=ACDelQuickStartTasksName, bStoredProcedure:=ACDelQuickStartTasksStored, lRecordsAffected:=lRecordsAffected)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteQuickStartTasksFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteQuickStartTasks", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Public Function DeleteSingleQuickStartTask(ByVal pmwrk_task_group_id As Integer,
                                      ByVal pmwrk_task_id As Integer,
                                      ByVal user_id As Integer) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase
                ' Clear previous parameters
                .Parameters.Clear()

                ' Add parameters for stored procedure
                m_lReturn = .Parameters.Add(sName:="pmwrk_task_group_id", vValue:=CStr(pmwrk_task_group_id),
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMInteger)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .Parameters.Add(sName:="pmwrk_task_id", vValue:=CStr(pmwrk_task_id),
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMInteger)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .Parameters.Add(sName:="user_id", vValue:=CStr(user_id),
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMInteger)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute stored procedure
                m_lReturn = .SQLAction(sSQL:=ACDelSingleQuickStartTasksSQL, sSQLName:=ACDelSingleQuickStartTasksName,
                                    bStoredProcedure:=ACDelSingleQuickStartTasksStored, lRecordsAffected:=lRecordsAffected)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log the error
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                           sMsg:="DeleteQuickStartTaskFailed", vApp:=ACApp, vClass:=ACClass,
                           vMethod:="DeleteQuickStartTask", vErrNo:=Information.Err().Number,
                           vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try

    End Function


    ' ***************************************************************** '
    ' Name: GetQuickStartTasks
    '
    ' Description: Gets all of the Quick Start Tasks for the current User.
    '
    ' ***************************************************************** '
    Public Function GetQuickStartTasks(ByRef r_vQuickStartArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="user_id", vValue:=CStr(m_iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .SQLSelect(sSQL:=ACGetQuickStartTasksSQL, sSQLName:=ACGetQuickStartTasksName, bStoredProcedure:=ACGetQuickStartTasksStored, lnumberrecords:=gPMConstants.PMAllRecords, vResultArray:=r_vQuickStartArray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetQuickStartTasksFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetQuickStartTasks", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: LockTaskInstance
    '
    ' Description: Locks the Task Instance Supplied.
    ' ***************************************************************** '
    Public Function LockTaskInstance(ByVal v_lPMWrkTaskInstanceCnt As Integer, ByRef r_sCurrentlyLockedBy As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Lock the Task Instance


            Return m_oLock.LockKey(sKeyName:=ACLockName, vkeyvalue:=v_lPMWrkTaskInstanceCnt, iUserID:=m_iUserID, scurrentlylockedby:=r_sCurrentlyLockedBy)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LockTaskInstanceFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="LockTaskInstance", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UnlockTaskInstance
    '
    ' Description: Unlocks the Task Instance Supplied.
    ' ***************************************************************** '
    Public Function UnlockTaskInstance(ByVal v_lPMWrkTaskInstanceCnt As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Unlock the Task Instance


            Return m_oLock.UnLockKey(sKeyName:=ACLockName, vkeyvalue:=v_lPMWrkTaskInstanceCnt, iUserID:=m_iUserID)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnlockTaskInstanceFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnlockTaskInstance", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetPMNewsWebAddress
    '
    ' Description: Returns the PMNews Web Address, which is in the
    '              Registry on the SERVER.
    ' ***************************************************************** '
    Public Function GetPMNewsWebAddress() As String

        Dim result As String = String.Empty
        Dim sWebAddress As String = ""

        Try

            result = ""

            m_lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:=gPMConstants.ACWrkManRegWebAddress, r_sSettingValue:=sWebAddress, v_sSubKey:=gPMConstants.ACWrkManRegSubKey)


            Return sWebAddress

        Catch




            Return ""
        End Try

    End Function

    ' ***************************************************************** '
    ' Name: SetPMNewsWebAddress
    '
    ' Description: Sets the PMNews Web Address, in the
    '              Registry on the SERVER.
    ' ***************************************************************** '
    Public Function SetPMNewsWebAddress(ByVal v_sWebAddress As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:=gPMConstants.ACWrkManRegWebAddress, v_sSettingValue:=v_sWebAddress, v_sSubKey:=gPMConstants.ACWrkManRegSubKey)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetPMNewsWebAddressFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetPMNewsWebAddress", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: CreateTaskInstance
    '
    ' Description:
    '
    ' History: 14/10/1999 DAK - Created.
    '
    ' ***************************************************************** '
    'DAK141299
    Public Function CreateTaskInstance(ByVal v_lPMWrkTaskGroupID As Integer, ByVal v_lPMWrkTaskID As Integer, ByVal v_sCustomer As String, ByVal v_dtTaskDueDate As Date, ByVal v_lPmuserGroupID As Integer, ByVal v_sDescription As String, ByVal v_iTaskStatus As Integer, ByVal v_iIsUrgent As Integer, ByRef r_lPMWrkTaskInstanceCnt As Integer, Optional ByVal v_iIsVisible As Integer = gPMConstants.PMEReturnCode.PMTrue) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'DAK141299

            m_lReturn = m_oTaskControl.CreateNew(v_lPMWrkTaskGroupID:=v_lPMWrkTaskGroupID, v_lPMWrkTaskID:=v_lPMWrkTaskID, v_sCustomer:=v_sCustomer, v_dtTaskDueDate:=v_dtTaskDueDate, v_lPmuserGroupID:=v_lPmuserGroupID, v_sDescription:=v_sDescription, v_iTaskStatus:=v_iTaskStatus, v_iIsUrgent:=v_iIsUrgent, r_lPMWrkTaskInstanceCnt:=r_lPMWrkTaskInstanceCnt, v_iUserID:=m_iUserID, v_iIsVisible:=v_iIsVisible)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateTaskInstance Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateTaskInstance", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetWebTabCaption
    '
    ' Description: Gets the caption for the Web Tab displayed on Work
    '              Manager.
    '
    ' History: 22/06/2000 DAK - Created.
    '
    ' ***************************************************************** '
    Public Function GetWebTabCaption() As String
        Dim result As String = String.Empty
        Dim sWebTabCaption As String = ""


        Try

            result = ""

            m_lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:=gPMConstants.ACWrkManRegWebTabCaption, r_sSettingValue:=sWebTabCaption, v_sSubKey:=gPMConstants.ACWrkManRegSubKey)


            Return sWebTabCaption

        Catch excep As System.Exception



            result = ""

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetWebTabCaption Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetWebTabCaption", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: SetWebTabCaption
    '
    ' Description: Sets the caption for the Web Tab displayed on Work
    '              Manager.
    '
    ' History: 19/06/2000 DAK - Created.
    '
    ' ***************************************************************** '
    Public Function SetWebTabCaption(ByVal v_sWebTabCaption As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:=gPMConstants.ACWrkManRegWebTabCaption, v_sSettingValue:=v_sWebTabCaption, v_sSubKey:=gPMConstants.ACWrkManRegSubKey)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetWebTabCaption Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetWebTabCaption", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    ' RDC 16052002 get data for website buttons
    Public Function GetToolBarWebsiteData(ByRef vButtonData(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.SQLSelect(sSQL:="spu_pm_wrk_get_websites", sSQLName:="GetButtonData", bStoredProcedure:=True, vResultArray:=vButtonData)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            If Not Information.IsArray(vButtonData) Then
                Return result
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch



            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function


    'DJM 10/03/2004
    Public Function GetDefaultUserGroupForTaskGroup(ByVal v_iUserID As Integer, ByVal v_lTaskGroupID As Integer, ByRef v_lUserGroupID As Integer) As Integer

        Dim result As Integer = 0
        Dim vResults(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="user_id", vValue:=CStr(v_iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .Parameters.Add(sName:="task_group_id", vValue:=CStr(v_lTaskGroupID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .SQLSelect(sSQL:=ACGetDefaultUserGroupForTaskGroupSQL, sSQLName:=ACGetDefaultUserGroupForTaskGroupName, bStoredProcedure:=ACGetDefaultUserGroupForTaskGroupStored, vResultArray:=vResults)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If Information.IsArray(vResults) Then

                    v_lUserGroupID = CInt(vResults(0, 0))
                End If

            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDefaultUserGroupForTaskGroupFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDefaultUserGroupForTaskGroup", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function GetAgents(ByRef r_vArray(,) As Object, ByRef r_iCurrentAgent As Integer) As Integer
        Dim result As Integer = 0
        Dim sUserGroups As New StringBuilder

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            bPMAddParameter.AddParameterLite(m_oDatabase, "User_ID", m_iUserID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, True)
            bPMAddParameter.AddParameterLite(m_oDatabase, "Current_Party_Cnt", "", gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMInteger)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAgentTaskSQL, sSQLName:=ACGetAgentTaskName, bStoredProcedure:=ACGetAgentTaskStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vArray)
            r_iCurrentAgent = gPMFunctions.NullToInteger(m_oDatabase.Parameters.Item("Current_Party_Cnt").Value)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                r_vArray = Nothing
                Return result
            End If

            If Not Information.IsArray(r_vArray) Then
                r_vArray = Nothing
                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAgentTask", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAgents", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try


    End Function
    ''' <summary>
    ''' Get User Authority for View Batch Task Status
    ''' </summary>
    ''' <param name="r_oResultArray"></param>
    ''' <returns></returns>
    Public Function GetViewBatchProcessStatusUserAuthority(ByRef r_oResultArray(,) As Object) As Integer
        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMTrue
        Const sMethodName As String = "GetViewBatchProcessStatusUserAuthority"
        Try

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=CStr(m_iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                gPMFunctions.RaiseError(sMethodName, "Failed to Add parameter user_id")
                Return nResult
            End If

            'Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=kACGetViewBatchProcessStatusAuthoritySQL, sSQLName:=kACGetViewBatchProcessStatusAuthorityName, bStoredProcedure:=kACGetViewBatchProcessStatusAuthorityStored, vResultArray:=r_oResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                Return nResult
            End If

        Catch excep As Exception
            nResult = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetViewBatchProcessStatusUserAuthority Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetViewBatchProcessStatus", excep:=excep)
        End Try

        Return nResult
    End Function

End Class
