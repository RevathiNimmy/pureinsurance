Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports SharedFiles


<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
Public NotInheritable Class Business
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness

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


    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date:
    '
    ' Description: Creatable Business class which contains all the
    '              methods, Business rules required.
    '
    '
    ' History:
    ' ***************************************************************** '

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Business"


    Private m_oDatabase As dPMDAO.Database

    Private m_oArcDatabase As dPMDAO.Database
    Private m_bCloseDatabase As Boolean
    Private m_bCloseArcDatabase As Boolean
    Private m_lCurrentRecord As Integer
    Private m_lReturn As gPMConstants.PMEReturnCode

    '*************************
    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As gPMConstants.PMEProcessMode
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    '*************************

    Private m_oLookup As Object

    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        End Get
    End Property

    ' ***************************************************************** '
    ' Name: GetLookupValues
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 02-06-2003 : 223
    ' ***************************************************************** '
    Public Function GetLookupValues(ByRef r_vTableArray(,) As Object, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "GetLookupValues"

        Dim dtEffectiveDate As Date

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the default effective Date
            dtEffectiveDate = DateTime.Now

            ' Get the Lookup items

            If m_oLookup.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAll, vTableArray:=r_vTableArray, iLanguageID:=m_iLanguageID, dtEffectiveDate:=dtEffectiveDate, vResultArray:=r_vResultArray) <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse

                LogMsg(v_sMsg:="Failed to retrieve lookup values", v_sMethod:=sFunctionName)

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
            '******************************

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: LogMsg
    '
    ' Parameters: n/a
    '
    ' Description: Wrapper for default LogMessageToFile function
    '
    ' History:
    '           Created : MEvans : 28-05-2003 : 223
    ' ***************************************************************** '
    Private Sub LogMsg(ByVal v_sMsg As String, ByVal v_sMethod As String)

        Const sFunctionName As String = "LogMsg"



        gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=v_sMethod & ":" & v_sMsg, vApp:=ACApp, vClass:=ACClass, vMethod:=v_sMethod)


    End Sub



    ' ***************************************************************** '
    ' Name: GetPMUser
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 01-07-2003 : workflow
    ' ***************************************************************** '
    Public Function GetPMUser(ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "GetPMUser"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters

            m_oDatabase.Parameters.Clear()

            ' Execute selection Query

            If m_oDatabase.SQLSelect(sSQL:=ACGetPMUserDetailsSQL, sSQLName:=ACGetPMUserDetailsName, bStoredProcedure:=True, vResultArray:=r_vResults) <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse

                '******************************
                ' Log Error.
                gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to retrieve details from table PMUser", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=New Exception(Information.Err().Description))
                '******************************

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
            '******************************


            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetPMUserGroupUsers
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 01-07-2003 : workflow
    ' ***************************************************************** '
    Public Function GetPMUserGroupUsers(ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "GetPMUserGroupUsers"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters

            m_oDatabase.Parameters.Clear()

            ' Execute selection Query

            If m_oDatabase.SQLSelect(sSQL:=ACGetPMUserGroupUsersSQL, sSQLName:=ACGetPMUserGroupUsersName, bStoredProcedure:=True, vResultArray:=r_vResults) <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse

                '******************************
                ' Log Error.
                gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to retrieve details from PMUSer_Group_User", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=New Exception(Information.Err().Description))
                '******************************

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
            '******************************


            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetTaskGroupTaskAction
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 01-07-2003 : workflow
    ' ***************************************************************** '
    Public Function GetTaskGroupTaskAction(ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "GetTaskGroupTaskAction"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters

            m_oDatabase.Parameters.Clear()

            ' Execute selection Query

            If m_oDatabase.SQLSelect(sSQL:=ACGetTaskGroupTaskActionSQL, sSQLName:=ACGetTaskGroupTaskActionName, bStoredProcedure:=True, vResultArray:=r_vResults) <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse

                '******************************
                ' Log Error.
                gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to retrieve details from PMWrk_Task_Group_Task_Action", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=New Exception(Information.Err().Description))
                '******************************

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
            '******************************


            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetTaskGroupTask
    '
    ' Parameters: n/a
    '
    ' Description: Returns all entries from  link table Pmwrk_task_group_task
    '
    ' History:
    '           Created : MEvans : 01-07-2003 : workflow
    ' ***************************************************************** '
    Public Function GetTaskGroupTask(ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "GetTaskGroupTask"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters

            m_oDatabase.Parameters.Clear()

            ' Execute selection Query

            If m_oDatabase.SQLSelect(sSQL:=ACGetTaskGroupTaskSQL, sSQLName:=ACGetTaskGroupTaskName, bStoredProcedure:=True, vResultArray:=r_vResults) <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse

                '******************************
                ' Log Error.
                gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to retrieve details from PMWrk_Task_Group_Task", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=New Exception(Information.Err().Description))
                '******************************

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
            '******************************

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetTaskGroupUserGroups
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 01-07-2003 : workflow
    ' ***************************************************************** '
    Public Function GetTaskGroupUserGroups(ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "GetTaskGroupUserGroups"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters

            m_oDatabase.Parameters.Clear()

            ' Execute selection Query

            If m_oDatabase.SQLSelect(sSQL:=ACGetTaskGroupUserGroupsSQL, sSQLName:=ACGetTaskGroupUserGroupsName, bStoredProcedure:=True, vResultArray:=r_vResults) <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse

                '******************************
                ' Log Error.
                gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to retrieve details from PMUser_Group_User", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=New Exception(Information.Err().Description))
                '******************************

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
            '******************************

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetTaskActionTypeOutcomes
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 01-07-2003 : workflow
    ' ***************************************************************** '
    Public Function GetTaskActionTypeOutcomes(ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "GetTaskActionTypeOutcomes"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters

            m_oDatabase.Parameters.Clear()

            ' Execute selection Query

            If m_oDatabase.SQLSelect(sSQL:=ACGetTaskActionTypeOutcomesSQL, sSQLName:=ACGetTaskActionTypeOutcomesName, bStoredProcedure:=True, vResultArray:=r_vResults) <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse

                '******************************
                ' Log Error.
                gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to retrieve details from PMWrk_Task_Action_Type_Outcome", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=New Exception(Information.Err().Description))
                '******************************

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
            '******************************

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetSpecifiedEventTask
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 02-07-2003 : workflow
    ' ***************************************************************** '
    Public Function GetSpecifiedEventTask(ByRef r_vResults(,) As Object, ByVal v_lTaskInstanceCnt As Integer) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "GetSpecifiedEventTask"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters

            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = CType(AddInputParameter("PMWrk_Task_Instance_Cnt", v_lTaskInstanceCnt, gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' Execute selection Query

            If m_oDatabase.SQLSelect(sSQL:=ACGetEventTaskSQL, sSQLName:=ACGetEventTaskName, bStoredProcedure:=True, vResultArray:=r_vResults) <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse

                '******************************
                ' Log Error.
                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("v_lTaskInstanceCnt", v_lTaskInstanceCnt)
                gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to retrieve event task details for " & "pmwrk_task_instance_cnt" & CStr(v_lTaskInstanceCnt), vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=New Exception(Information.Err().Description), oDicParms:=oDict)
                '******************************

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error.
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_lTaskInstanceCnt", v_lTaskInstanceCnt)
            gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep, oDicParms:=oDict)
            '******************************

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CreateTask
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 03-07-2003 : workflow
    ' ***************************************************************** '
    Function CreateTask(ByVal v_lTaskGroupID As Integer, ByVal v_lTaskID As Integer, ByVal v_lTaskActionTypeID As Integer, ByVal v_sTaskCustomer As String, ByVal v_dtTaskDueDate As Date, ByVal v_lTaskPMUserGroupID As Integer, ByVal v_sTaskDescription As String, ByVal v_iTaskStatus As Integer, ByVal v_iTaskIsUrgent As Integer, ByRef r_lTaskInstanceCnt As Integer, Optional ByVal v_iTaskIsVisible As Integer = 0, Optional ByVal v_sTaskWorkflowInformation As String = "", Optional ByVal v_iTaskUserID As Integer = 0, Optional ByVal v_vTaskInstanceKeyArray As Object = Nothing, Optional ByVal v_dtTaskOutcomeDate As Date = #12/30/1899#, Optional ByVal v_lTaskOutcomeId As Integer = 0, Optional ByVal v_lCallingProcessKeyId As Integer = 0, Optional ByVal v_lCallingProcessType As Integer = 0, Optional ByVal v_iUseWorkTables As Integer = 0, Optional ByVal v_iTaskSourceId As Integer = 1) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "CreateTask"

        Dim oWrkMgrTaskControl As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' create instance of business object
            If gPMComponentServices.CreateBusinessObject(r_oObject:=oWrkMgrTaskControl, v_sClassName:="bPMWrkTaskInstance.TaskControl", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase) = gPMConstants.PMEReturnCode.PMTrue Then

                ' create new task

                If oWrkMgrTaskControl.CreateNew(v_lPMWrkTaskGroupID:=v_lTaskGroupID, v_lPMWrkTaskID:=v_lTaskID, v_sCustomer:=v_sTaskCustomer, v_dtTaskDueDate:=v_dtTaskDueDate, v_lPMUserGroupID:=v_lTaskPMUserGroupID, v_sDescription:=v_sTaskDescription, v_iTaskStatus:=v_iTaskStatus, v_iIsUrgent:=v_iTaskIsUrgent, r_lPMWrkTaskInstanceCnt:=r_lTaskInstanceCnt, v_iSourceID:=v_iTaskSourceId, v_sWorkflowInformation:=v_sTaskWorkflowInformation, v_iUserID:=v_iTaskUserID, v_vKeyArray:=v_vTaskInstanceKeyArray, v_iIsVisible:=v_iTaskIsVisible, v_lCallingProcessKeyId:=v_lCallingProcessKeyId, v_lCallingProcessType:=v_lCallingProcessType, v_iUseWorkTables:=v_iUseWorkTables, v_lPMWrkTaskActionTypeID:=ConvertToNullValues(CStr(v_lTaskActionTypeID), gPMConstants.PMEDataType.PMLong), v_dteTaskOutcomeDate:=ConvertToNullValues(DateTimeHelper.ToString(v_dtTaskOutcomeDate), gPMConstants.PMEDataType.PMDate), v_lTaskOutcomeId:=ConvertToNullValues(CStr(v_lTaskOutcomeId), gPMConstants.PMEDataType.PMLong)) <> gPMConstants.PMEReturnCode.PMTrue Then

                    ' log error
                    result = gPMConstants.PMEReturnCode.PMFalse

                    LogMsg(v_sMsg:="Failed to create task", v_sMethod:=sFunctionName)

                End If

            Else

                ' log error
                result = gPMConstants.PMEReturnCode.PMFalse

                LogMsg(v_sMsg:="Failed to create instance of bPMWrkTaskInstance.Business", v_sMethod:=sFunctionName)

            End If

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error.
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_lTaskGroupID", v_lTaskGroupID)
            oDict.Add("v_lTaskID", v_lTaskID)
            oDict.Add("v_lTaskActionTypeID", v_lTaskActionTypeID)
            oDict.Add("v_dtTaskDueDate", v_dtTaskDueDate)
            oDict.Add("v_lTaskPMUserGroupID", v_lTaskPMUserGroupID)
            oDict.Add("r_lTaskInstanceCnt", r_lTaskInstanceCnt)
            oDict.Add("v_iTaskUserID", v_iTaskUserID)
            oDict.Add("v_lTaskOutcomeId", v_lTaskOutcomeId)
            oDict.Add("v_lCallingProcessKeyId", v_lCallingProcessKeyId)
            oDict.Add("v_iTaskSourceId", v_iTaskSourceId)
            gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep, oDicParms:=oDict)
            '******************************


        End Try



        Return result


        Return result
    End Function

    ' ***************************************************************** '
    ' Name: ConvertToNullValues
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 05-06-2003 : 223
    ' ***************************************************************** '
    Private Function ConvertToNullValues(ByRef r_vValue As String, ByVal v_iDataType As Integer) As String

        Dim result As String = String.Empty
        Const sFunctionName As String = "ConvertToNullValues"


        Dim vReturn As String = ""



        result = CStr(gPMConstants.PMEReturnCode.PMTrue)

        If False Then
            vReturn = r_vValue
        Else


            Select Case v_iDataType
                Case gPMConstants.PMEDataType.PMLong, gPMConstants.PMEDataType.PMCurrency
                    If StringsHelper.ToDoubleSafe(r_vValue) = 0 Then

                        vReturn = Nothing
                    Else
                        vReturn = r_vValue
                    End If

                Case gPMConstants.PMEDataType.PMDate
                    If StringsHelper.ToDoubleSafe(r_vValue) = 0 Or r_vValue = "00:00:00" Then

                        vReturn = Nothing
                    Else
                        vReturn = r_vValue
                    End If

                Case gPMConstants.PMEDataType.PMString
                    If r_vValue = "" Then

                        vReturn = Nothing
                    Else
                        vReturn = r_vValue
                    End If

            End Select

        End If

        Return vReturn

    End Function

    ' ***************************************************************** '
    ' Name: CreateEvent
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 03-07-2003 : workflow
    ' ***************************************************************** '
    Function CreateEvent(ByRef r_vEventCnt As Object, Optional ByVal v_vEventPartyCnt As Object = Nothing, Optional ByVal v_vEventInsuranceFolderCnt As Object = Nothing, Optional ByVal v_vEventInsuranceFileCnt As Object = Nothing, Optional ByVal v_vEventClaimCnt As Object = Nothing, Optional ByVal v_vEventDocumentCnt As Object = Nothing, Optional ByVal v_vEventOldAddressCnt As Object = Nothing, Optional ByVal v_vEventNewAddressCnt As Object = Nothing, Optional ByVal v_vEventCampaignId As Object = Nothing, Optional ByVal v_vEventDocumentType As Object = Nothing, Optional ByVal v_vEventReportType As Object = Nothing, Optional ByVal v_vEventType As Object = Nothing, Optional ByVal v_vEventDescription As Object = Nothing, Optional ByVal v_vEventOldPartyTypeID As Object = Nothing, Optional ByVal v_vEventLogSubjectId As Object = Nothing, Optional ByVal v_vEventTypeCode As Object = Nothing, Optional ByVal v_vEventAccountKey As Object = Nothing, Optional ByVal v_vEventDocumentTemplateID As Object = Nothing, Optional ByVal v_vEventOutputMediaCode As Object = Nothing, Optional ByVal v_lEventClaimDebtID As Object = Nothing, Optional ByVal v_lEventPMWrkTaskInstanceCnt As Integer = 0, Optional ByVal v_lCallingProcessKeyId As Integer = 0, Optional ByVal v_lCallingProcessType As Integer = 0, Optional ByVal v_iUseWorkTables As Integer = 0, Optional ByVal v_lClaimPartyId As Integer = 0, Optional ByVal v_vEventClaimPerilId As Object = Nothing) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "CreateEvent"

        Dim oEventBusiness As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' create an instance of the component
            If gPMComponentServices.CreateBusinessObject(r_oObject:=oEventBusiness, v_sClassName:="bSIREvent.Business", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase) = gPMConstants.PMEReturnCode.PMTrue Then

                ' create event




















                If oEventBusiness.DirectAdd(vEventCnt:=r_vEventCnt, vPartyCnt:=v_vEventPartyCnt, vInsuranceFolderCnt:=ConvertToNullValues(CStr(v_vEventInsuranceFolderCnt), gPMConstants.PMEDataType.PMLong), vInsuranceFileCnt:=ConvertToNullValues(CStr(v_vEventInsuranceFileCnt), gPMConstants.PMEDataType.PMLong), vClaimCnt:=ConvertToNullValues(CStr(v_vEventClaimCnt), gPMConstants.PMEDataType.PMLong), vDocumentCnt:=ConvertToNullValues(CStr(v_vEventDocumentCnt), gPMConstants.PMEDataType.PMLong), vOldAddressCnt:=ConvertToNullValues(CStr(v_vEventOldAddressCnt), gPMConstants.PMEDataType.PMLong), vNewAddressCnt:=ConvertToNullValues(CStr(v_vEventNewAddressCnt), gPMConstants.PMEDataType.PMLong), vCampaignId:=ConvertToNullValues(CStr(v_vEventCampaignId), gPMConstants.PMEDataType.PMLong), vDocumentType:=ConvertToNullValues(CStr(v_vEventDocumentType), gPMConstants.PMEDataType.PMLong), vReportType:=ConvertToNullValues(CStr(v_vEventReportType), gPMConstants.PMEDataType.PMLong), vEventType:=ConvertToNullValues(CStr(v_vEventType), gPMConstants.PMEDataType.PMLong), vUserId:=m_iUserID, vEventDate:=DateTime.Now, vDescription:=ConvertToNullValues(CStr(v_vEventDescription), gPMConstants.PMEDataType.PMString), vOldPartyTypeID:=ConvertToNullValues(CStr(v_vEventOldPartyTypeID), gPMConstants.PMEDataType.PMLong), vEventLogSubjectId:=ConvertToNullValues(CStr(v_vEventLogSubjectId), gPMConstants.PMEDataType.PMLong), vEventTypeCode:=ConvertToNullValues(CStr(v_vEventTypeCode), gPMConstants.PMEDataType.PMString), vAccountKey:=ConvertToNullValues(CStr(v_vEventAccountKey), gPMConstants.PMEDataType.PMLong), vDocumentTemplateID:=ConvertToNullValues(CStr(v_vEventDocumentTemplateID), gPMConstants.PMEDataType.PMLong), vOutputMediaCode:=ConvertToNullValues(CStr(v_vEventOutputMediaCode), gPMConstants.PMEDataType.PMString), v_lClaimDebtID:=ConvertToNullValues(CStr(v_lEventClaimDebtID), gPMConstants.PMEDataType.PMLong), v_lPMWrkTaskInstanceCnt:=ConvertToNullValues(CStr(v_lEventPMWrkTaskInstanceCnt), gPMConstants.PMEDataType.PMLong), v_lCallingProcessKeyId:=ConvertToNullValues(CStr(v_lCallingProcessKeyId), gPMConstants.PMEDataType.PMLong), v_lCallingProcessType:=ConvertToNullValues(CStr(v_lCallingProcessType), gPMConstants.PMEDataType.PMLong), v_iUseWorkTables:=v_iUseWorkTables, vClaimPartyId:=ConvertToNullValues(CStr(v_lClaimPartyId), gPMConstants.PMEDataType.PMLong), vClaimPerilId:=ConvertToNullValues(CStr(v_vEventClaimPerilId), gPMConstants.PMEDataType.PMLong)) <> gPMConstants.PMEReturnCode.PMTrue Then

                    ' log error
                    result = gPMConstants.PMEReturnCode.PMFalse

                    LogMsg(v_sMsg:="Failed to create event", v_sMethod:=sFunctionName)

                End If


            Else

                ' log error
                result = gPMConstants.PMEReturnCode.PMFalse

                LogMsg(v_sMsg:="Failed to create instance of bSIREvent.Business", v_sMethod:=sFunctionName)

            End If

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
            '*******************************


        End Try



        Return result


        Return result
    End Function


    ' ***************************************************************** '
    ' Name: CreateEventTask
    '
    ' Parameters: n/a
    '
    ' Description: Creates an event, task and any required documents.
    '
    ' History:
    '           Created : MEvans : 03-06-2003 : workflow
    '           Updated : RVH - 16/09/2003 - Changed non-mandatory
    '                     parameters to optional, forcing re-order.
    ' ***************************************************************** '
    Public Function CreateEventTask(ByVal v_lTaskGroupID As Integer, ByVal v_lTaskID As Integer, ByVal v_lTaskActionTypeID As Integer, ByVal v_sTaskCustomer As String, ByVal v_dtTaskDueDate As Date, ByVal v_lTaskPMUserGroupID As Integer, ByVal v_sTaskDescription As String, ByVal v_iTaskStatus As Integer, ByVal v_iTaskIsUrgent As Integer, ByVal v_iTaskUserID As Integer, ByVal v_vEventPartyCnt As Object, ByVal v_vEventClaimCnt As Object, ByVal v_vEventDescription As Object, ByVal v_vEventTypeCode As String, ByRef r_lTaskInstanceCnt As Integer, ByRef r_vEventCnt As Object, Optional ByVal v_vEventInsuranceFolderCnt As Object = Nothing, Optional ByVal v_vEventInsuranceFileCnt As Object = Nothing, Optional ByVal v_vEventLogSubjectId As Object = Nothing, Optional ByVal v_vEventType As Object = Nothing, Optional ByVal v_dtTaskOutcomeDate As Date = #12/30/1899#, Optional ByVal v_lTaskOutcomeId As Integer = 0, Optional ByVal v_vTaskInstanceKeyArray As Object = Nothing, Optional ByVal v_iTaskIsVisible As Integer = 0, Optional ByVal v_sTaskWorkflowInformation As String = "", Optional ByVal v_vEventOldAddressCnt As Object = Nothing, Optional ByVal v_vEventNewAddressCnt As Object = Nothing, Optional ByVal v_vEventCampaignId As Object = Nothing, Optional ByVal v_vEventDocumentType As Object = Nothing, Optional ByVal v_vEventReportType As Object = Nothing, Optional ByVal v_vEventOldPartyTypeID As Object = Nothing, Optional ByVal v_vEventAccountKey As Object = Nothing, Optional ByVal v_vEventDocumentCnt As Object = Nothing, Optional ByVal v_vEventDocumentTemplateID As Object = Nothing, Optional ByVal v_vEventOutputMediaCode As Object = Nothing, Optional ByVal v_lEventClaimDebtID As Object = Nothing, Optional ByVal v_vEventClaimPerilId As Object = Nothing, Optional ByVal v_lCallingProcessKeyId As Integer = 0, Optional ByVal v_lCallingProcessType As Integer = 0, Optional ByVal v_iUseWorkTables As Integer = gPMConstants.PMEReturnCode.PMFalse, Optional ByVal v_lEventClaimPartyId As Integer = 0, Optional ByVal v_iTaskSourceId As Integer = 1) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "CreateEventTask"

        Try

            Dim bError As Boolean
            Dim lTaskInstanceCnt As Integer

            result = gPMConstants.PMEReturnCode.PMTrue

            ' if we have the basic task details
            If v_lTaskGroupID <> 0 And v_lTaskID <> 0 Then

                ' attempt to create the task
                If CreateTask(r_lTaskInstanceCnt:=lTaskInstanceCnt, v_lTaskGroupID:=v_lTaskGroupID, v_iTaskIsVisible:=v_iTaskIsVisible, v_lTaskID:=v_lTaskID, v_lTaskActionTypeID:=v_lTaskActionTypeID, v_sTaskCustomer:=v_sTaskCustomer, v_dtTaskDueDate:=v_dtTaskDueDate, v_lTaskPMUserGroupID:=v_lTaskPMUserGroupID, v_sTaskDescription:=v_sTaskDescription, v_iTaskStatus:=v_iTaskStatus, v_iTaskIsUrgent:=v_iTaskIsUrgent, v_sTaskWorkflowInformation:=v_sTaskWorkflowInformation, v_iTaskUserID:=v_iTaskUserID, v_vTaskInstanceKeyArray:=v_vTaskInstanceKeyArray, v_dtTaskOutcomeDate:=v_dtTaskOutcomeDate, v_lTaskOutcomeId:=v_lTaskOutcomeId, v_lCallingProcessKeyId:=v_lCallingProcessKeyId, v_lCallingProcessType:=v_lCallingProcessType, v_iUseWorkTables:=v_iUseWorkTables, v_iTaskSourceId:=v_iTaskSourceId) <> gPMConstants.PMEReturnCode.PMTrue Then

                    bError = True

                    ' log error
                    result = gPMConstants.PMEReturnCode.PMFalse

                    LogMsg(v_sMsg:="Failed to create task", v_sMethod:=sFunctionName)

                End If

            End If

            ' get the returned task instance cnt
            r_lTaskInstanceCnt = lTaskInstanceCnt

            ' dont create an event if no event type has been specified
            If v_vEventTypeCode <> "" Then

                If Not bError Then
                    ' create event
                    If CreateEvent(r_vEventCnt:=r_vEventCnt, v_vEventPartyCnt:=v_vEventPartyCnt, v_vEventInsuranceFolderCnt:=v_vEventInsuranceFolderCnt, v_vEventInsuranceFileCnt:=v_vEventInsuranceFileCnt, v_vEventClaimCnt:=v_vEventClaimCnt, v_vEventDocumentCnt:=v_vEventDocumentCnt, v_vEventOldAddressCnt:=v_vEventOldAddressCnt, v_vEventNewAddressCnt:=v_vEventNewAddressCnt, v_vEventCampaignId:=v_vEventCampaignId, v_vEventDocumentType:=v_vEventDocumentType, v_vEventReportType:=v_vEventReportType, v_vEventType:=v_vEventType, v_vEventDescription:=v_vEventDescription, v_vEventOldPartyTypeID:=v_vEventOldPartyTypeID, v_vEventLogSubjectId:=v_vEventLogSubjectId, v_vEventTypeCode:=v_vEventTypeCode, v_vEventAccountKey:=v_vEventAccountKey, v_vEventDocumentTemplateID:=v_vEventDocumentTemplateID, v_vEventOutputMediaCode:=v_vEventOutputMediaCode, v_lEventClaimDebtID:=v_lEventClaimDebtID, v_lEventPMWrkTaskInstanceCnt:=lTaskInstanceCnt, v_lCallingProcessKeyId:=v_lCallingProcessKeyId, v_lCallingProcessType:=v_lCallingProcessType, v_iUseWorkTables:=v_iUseWorkTables, v_lClaimPartyId:=v_lEventClaimPartyId, v_vEventClaimPerilId:=v_vEventClaimPerilId) <> gPMConstants.PMEReturnCode.PMTrue Then

                        ' log error
                        result = gPMConstants.PMEReturnCode.PMFalse

                        LogMsg(v_sMsg:="Failed to create event", v_sMethod:=sFunctionName)

                    End If

                End If

            End If
            '*************
            ' TODO Workflow 4.1.5.1 step 3a / step 3b
            '*************

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error.
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_lTaskGroupID", v_lTaskGroupID)
            oDict.Add("v_lTaskID", v_lTaskID)
            oDict.Add("v_lTaskActionTypeID", v_lTaskActionTypeID)
            oDict.Add("v_dtTaskDueDate", v_dtTaskDueDate)
            oDict.Add("v_lTaskPMUserGroupID", v_lTaskPMUserGroupID)
            oDict.Add("v_iTaskUserID", v_iTaskUserID)
            oDict.Add("v_vEventPartyCnt", v_vEventPartyCnt)
            oDict.Add("v_vEventClaimCnt", v_vEventClaimCnt)
            oDict.Add("v_vEventTypeCode", v_vEventTypeCode)
            oDict.Add("r_lTaskInstanceCnt", r_lTaskInstanceCnt)
            oDict.Add("r_vEventCnt", r_vEventCnt)
            oDict.Add("v_vEventInsuranceFolderCnt", v_vEventInsuranceFolderCnt)
            oDict.Add("v_vEventInsuranceFileCnt", v_vEventInsuranceFileCnt)
            oDict.Add("v_vEventLogSubjectId", v_vEventLogSubjectId)
            oDict.Add("v_dtTaskOutcomeDate", v_dtTaskOutcomeDate)
            oDict.Add("v_lTaskOutcomeId", v_lTaskOutcomeId)
            oDict.Add("v_vEventOldAddressCnt", v_vEventOldAddressCnt)
            oDict.Add("v_vEventNewAddressCnt", v_vEventNewAddressCnt)
            oDict.Add("v_vEventCampaignId", v_vEventCampaignId)
            oDict.Add("v_vEventOldPartyTypeID", v_vEventOldPartyTypeID)
            oDict.Add("v_vEventDocumentCnt", v_vEventDocumentCnt)
            oDict.Add("v_vEventDocumentTemplateID", v_vEventDocumentTemplateID)
            oDict.Add("v_vEventOutputMediaCode", v_vEventOutputMediaCode)
            oDict.Add("v_lEventClaimDebtID", v_lEventClaimDebtID)
            oDict.Add("v_vEventClaimPerilId", v_vEventClaimPerilId)
            oDict.Add("v_lCallingProcessKeyId", v_lCallingProcessKeyId)
            oDict.Add("v_lEventClaimPartyId", v_lEventClaimPartyId)
            oDict.Add("v_iTaskSourceId", v_iTaskSourceId)
            gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep, oDicParms:=oDict)
            '*******************************

            Return result


            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: AmendEventTask
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 04-07-2003 : workflow
    ' ***************************************************************** '
    Public Function AmendEventTask(ByVal v_lTaskInstanceCnt As Integer, ByVal v_sTaskCustomer As String, ByVal v_dtTaskDueDate As Date, ByVal v_sTaskDescription As String, ByVal v_iTaskStatus As Integer, ByVal v_iTaskIsUrgent As Integer, ByVal v_sTaskWorkflowInformation As String, ByVal v_dtTaskOutcomeDate As Date, ByVal v_lTaskOutcomeId As Integer, ByVal v_vEventCnt As Object, ByVal v_vEventPartyCnt As Object, ByVal v_vEventInsuranceFolderCnt As Object, ByVal v_vEventInsuranceFileCnt As Object, ByVal v_vEventClaimCnt As Object, ByVal v_vEventDocumentCnt As Object, ByVal v_vEventOldAddressCnt As Object, ByVal v_vEventNewAddressCnt As Object, ByVal v_vEventCampaignId As Object, ByVal v_vEventDocumentType As Object, ByVal v_vEventReportType As Object, ByVal v_vEventType As Object, ByVal v_vEventUserId As Object, ByVal v_vEventDate As Object, ByVal v_vEventDescription As Object, ByVal v_vEventOldPartyTypeID As Object, ByVal v_vEventDocumentTemplateID As Object, ByVal v_vEventOutputMediaCode As Object) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "AmendEventTask"

        Dim bError As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If AmendTask(v_lTaskInstanceCnt:=v_lTaskInstanceCnt, v_sCustomer:=v_sTaskCustomer, v_dtTaskDueDate:=v_dtTaskDueDate, v_sDescription:=v_sTaskDescription, v_iIsUrgent:=v_iTaskIsUrgent, v_sWorkflowInformation:=v_sTaskWorkflowInformation, v_iTaskStatus:=v_iTaskStatus, v_dtTaskOutcomeDate:=v_dtTaskOutcomeDate, v_lTaskOutcomeId:=v_lTaskOutcomeId) <> gPMConstants.PMEReturnCode.PMTrue Then

                bError = True

                result = gPMConstants.PMEReturnCode.PMFalse

                LogMsg(v_sMsg:="Failed to amend task instance : " & v_lTaskInstanceCnt, v_sMethod:=sFunctionName)

            End If

            If Not bError Then
                If AmendEvent(v_vEventCnt:=v_vEventCnt, v_vPartyCnt:=v_vEventPartyCnt, v_vInsuranceFolderCnt:=v_vEventInsuranceFolderCnt, v_vInsuranceFileCnt:=v_vEventInsuranceFileCnt, v_vClaimCnt:=v_vEventClaimCnt, v_vDocumentCnt:=v_vEventDocumentCnt, v_vOldAddressCnt:=v_vEventOldAddressCnt, v_vNewAddressCnt:=v_vEventNewAddressCnt, v_vCampaignId:=v_vEventCampaignId, v_vDocumentType:=v_vEventDocumentType, v_vReportType:=v_vEventReportType, v_vEventType:=v_vEventType, v_vUserId:=v_vEventUserId, v_vEventDate:=v_vEventDate, v_vDescription:=v_vEventDescription, v_vOldPartyTypeID:=v_vEventOldPartyTypeID, v_vDocumentTemplateID:=v_vEventDocumentTemplateID, v_vOutputMediaCode:=v_vEventOutputMediaCode, v_lPMWrkTaskInstanceCnt:=v_lTaskInstanceCnt) <> gPMConstants.PMEReturnCode.PMTrue Then

                    result = gPMConstants.PMEReturnCode.PMFalse


                    LogMsg(v_sMsg:="Failed to amend event cnt :" & CStr(v_vEventCnt), v_sMethod:=sFunctionName)

                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error.
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_lTaskInstanceCnt", v_lTaskInstanceCnt)
            oDict.Add("v_dtTaskDueDate", v_dtTaskDueDate)
            oDict.Add("v_dtTaskOutcomeDate", v_dtTaskOutcomeDate)
            oDict.Add("v_lTaskOutcomeId", v_lTaskOutcomeId)
            oDict.Add("v_vEventCnt", v_vEventCnt)
            oDict.Add("v_vEventPartyCnt", v_vEventPartyCnt)
            oDict.Add("v_vEventInsuranceFolderCnt", v_vEventInsuranceFolderCnt)
            oDict.Add("v_vEventInsuranceFileCnt", v_vEventInsuranceFileCnt)
            oDict.Add("v_vEventClaimCnt", v_vEventClaimCnt)
            oDict.Add("v_vEventDocumentCnt", v_vEventDocumentCnt)
            oDict.Add("v_vEventOldAddressCnt", v_vEventOldAddressCnt)
            oDict.Add("v_vEventNewAddressCnt", v_vEventNewAddressCnt)
            oDict.Add("v_vEventCampaignId", v_vEventCampaignId)
            oDict.Add("v_vEventUserId", v_vEventUserId)
            oDict.Add("v_vEventDate", v_vEventDate)
            oDict.Add("v_vEventOldPartyTypeID", v_vEventOldPartyTypeID)
            oDict.Add("v_vEventDocumentTemplateID", v_vEventDocumentTemplateID)
            oDict.Add("v_vEventOutputMediaCode", v_vEventOutputMediaCode)
            gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep, oDicParms:=oDict)
            '******************************

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: AmendEvent
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 04-07-2003 : workflow
    ' ***************************************************************** '
    Private Function AmendEvent(ByVal v_vEventCnt As Object, ByVal v_vPartyCnt As Object, ByVal v_vInsuranceFolderCnt As Object, ByVal v_vInsuranceFileCnt As Object, ByVal v_vClaimCnt As Object, ByVal v_vDocumentCnt As Object, ByVal v_vOldAddressCnt As Object, ByVal v_vNewAddressCnt As Object, ByVal v_vCampaignId As Object, ByVal v_vDocumentType As Object, ByVal v_vReportType As Object, ByVal v_vEventType As Object, ByVal v_vUserId As Object, ByVal v_vEventDate As Object, ByVal v_vDescription As Object, ByVal v_vOldPartyTypeID As Object, ByVal v_vDocumentTemplateID As Object, ByVal v_vOutputMediaCode As Object, ByVal v_lPMWrkTaskInstanceCnt As Object) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "AmendEvent"

        Dim oEvent As Object



        result = gPMConstants.PMEReturnCode.PMTrue

        ' create an instance of the component
        If gPMComponentServices.CreateBusinessObject(r_oObject:=oEvent, v_sClassName:="bSIREvent.Business", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase) = gPMConstants.PMEReturnCode.PMTrue Then


            ' amend event
















            If oEvent.DirectUpdate(vEventCnt:=v_vEventCnt, vPartyCnt:=v_vPartyCnt, vInsuranceFolderCnt:=ConvertToNullValues(CStr(v_vInsuranceFolderCnt), gPMConstants.PMEDataType.PMLong), vInsuranceFileCnt:=ConvertToNullValues(CStr(v_vInsuranceFileCnt), gPMConstants.PMEDataType.PMLong), vClaimCnt:=ConvertToNullValues(CStr(v_vClaimCnt), gPMConstants.PMEDataType.PMLong), vDocumentCnt:=ConvertToNullValues(CStr(v_vDocumentCnt), gPMConstants.PMEDataType.PMLong), vOldAddressCnt:=ConvertToNullValues(CStr(v_vOldAddressCnt), gPMConstants.PMEDataType.PMLong), vNewAddressCnt:=ConvertToNullValues(CStr(v_vNewAddressCnt), gPMConstants.PMEDataType.PMLong), vCampaignId:=ConvertToNullValues(CStr(v_vCampaignId), gPMConstants.PMEDataType.PMLong), vDocumentType:=ConvertToNullValues(CStr(v_vDocumentType), gPMConstants.PMEDataType.PMLong), vReportType:=ConvertToNullValues(CStr(v_vReportType), gPMConstants.PMEDataType.PMLong), vEventType:=ConvertToNullValues(CStr(v_vEventType), gPMConstants.PMEDataType.PMLong), vUserId:=ConvertToNullValues(CStr(v_vUserId), gPMConstants.PMEDataType.PMLong), vEventDate:=ConvertToNullValues(CStr(v_vEventDate), gPMConstants.PMEDataType.PMDate), vDescription:=v_vDescription, vOldPartyTypeID:=ConvertToNullValues(CStr(v_vOldPartyTypeID), gPMConstants.PMEDataType.PMLong), vDocumentTemplateID:=ConvertToNullValues(CStr(v_vDocumentTemplateID), gPMConstants.PMEDataType.PMLong), vOutputMediaCode:=v_vOutputMediaCode, v_lPMWrkTaskInstanceCnt:=ConvertToNullValues(CStr(v_lPMWrkTaskInstanceCnt), gPMConstants.PMEDataType.PMLong)) <> gPMConstants.PMEReturnCode.PMTrue Then

                ' log error
                result = gPMConstants.PMEReturnCode.PMFalse


                LogMsg(v_sMsg:="Failed to amend event :" & CStr(v_vEventCnt), v_sMethod:=sFunctionName)

            End If

        Else

            ' log error
            result = gPMConstants.PMEReturnCode.PMFalse

            LogMsg(v_sMsg:="Failed to create instance of bSIREvent.Business", v_sMethod:=sFunctionName)

        End If



        ' destroy object reference
        oEvent = Nothing

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: AmendTask
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 04-07-2003 : workflow
    ' ***************************************************************** '
    Private Function AmendTask(ByVal v_lTaskInstanceCnt As Integer, ByVal v_sCustomer As String, ByVal v_dtTaskDueDate As Date, ByVal v_sDescription As String, ByVal v_iIsUrgent As Integer, ByVal v_sWorkflowInformation As String, ByVal v_iTaskStatus As Integer, ByVal v_dtTaskOutcomeDate As Date, ByVal v_lTaskOutcomeId As Integer) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "AmendTask"

        Dim oTaskInstance As Object



        result = gPMConstants.PMEReturnCode.PMTrue

        ' create instance of business object
        If gPMComponentServices.CreateBusinessObject(r_oObject:=oTaskInstance, v_sClassName:="bPMWrkTaskInstance.TaskControl", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase) = gPMConstants.PMEReturnCode.PMTrue Then

            ' create new task

            If oTaskInstance.AmendDetails(v_lPMWrkTaskInstanceCnt:=v_lTaskInstanceCnt, v_sCustomer:=v_sCustomer, v_dtTaskDueDate:=v_dtTaskDueDate, v_sDescription:=v_sDescription, v_iIsUrgent:=v_iIsUrgent, v_sWorkflowInformation:=v_sWorkflowInformation, v_iTaskStatus:=v_iTaskStatus, v_dteTaskOutcomeDate:=ConvertToNullValues(DateTimeHelper.ToString(v_dtTaskOutcomeDate), gPMConstants.PMEDataType.PMDate), v_lTaskOutcomeId:=ConvertToNullValues(CStr(v_lTaskOutcomeId), gPMConstants.PMEDataType.PMLong)) <> gPMConstants.PMEReturnCode.PMTrue Then

                ' log error
                result = gPMConstants.PMEReturnCode.PMFalse

                LogMsg(v_sMsg:="Failed to amend task instance: " & v_lTaskInstanceCnt, v_sMethod:=sFunctionName)

            End If

        Else

            ' log error
            result = gPMConstants.PMEReturnCode.PMFalse

            LogMsg(v_sMsg:="Failed to create instance of bPMWrkTaskInstance.Business", v_sMethod:=sFunctionName)

        End If



        ' destroy object reference
        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetEventTask
    '
    ' Parameters: n/a
    '
    ' Description: finds any event Tasks matching optional parameters
    '
    ' History:
    '           Created : JMF : 30-06-2003 : workflow
    ' ***************************************************************** '
    Public Function GetEventTask(ByRef r_vResults(,) As Object, Optional ByVal v_lPMWrkTaskInstanceCnt As Integer = 0, Optional ByVal v_lClaimDebtID As Integer = 0, Optional ByVal v_lInsuranceFileCnt As Integer = 0, Optional ByVal v_lClaimCnt As Integer = 0, Optional ByVal v_lCallingProcessKeyId As Integer = 0, Optional ByVal v_lCallingProcessType As Integer = 0, Optional ByVal v_iUseWorkTables As Integer = gPMConstants.PMEReturnCode.PMFalse) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            .Parameters.Clear()



            If .Parameters.Add("pmwrk_task_action_type_id", IIf(False, DBNull.Value, v_lPMWrkTaskInstanceCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("v_lPMWrkTaskInstanceCnt", v_lPMWrkTaskInstanceCnt)
                oDict.Add("v_lClaimDebtID", v_lClaimDebtID)
                oDict.Add("v_lInsuranceFileCnt", v_lInsuranceFileCnt)
                oDict.Add("v_lClaimCnt", v_lClaimCnt)
                oDict.Add("v_lCallingProcessKeyId", v_lCallingProcessKeyId)
                gPMFunctions.LogMessageToFile(sMsg:="Parameters add  Failed", vMethod:="GetEventTask", sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, vApp:=ACApp, vClass:=ACClass, excep:=New Exception(Information.Err().Description), oDicParms:=oDict)
            End If


            If .Parameters.Add("pmwrk_task_action_type_id", IIf(False, DBNull.Value, v_lClaimDebtID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("v_lPMWrkTaskInstanceCnt", v_lPMWrkTaskInstanceCnt)
                oDict.Add("v_lClaimDebtID", v_lClaimDebtID)
                oDict.Add("v_lInsuranceFileCnt", v_lInsuranceFileCnt)
                oDict.Add("v_lClaimCnt", v_lClaimCnt)
                oDict.Add("v_lCallingProcessKeyId", v_lCallingProcessKeyId)
                gPMFunctions.LogMessageToFile(sMsg:="Parameters add  Failed", vMethod:="GetEventTask", sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, vApp:=ACApp, vClass:=ACClass, excep:=New Exception(Information.Err().Description), oDicParms:=oDict)
            End If


            If .Parameters.Add("pmwrk_task_action_type_id", IIf(False, DBNull.Value, v_lInsuranceFileCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("v_lPMWrkTaskInstanceCnt", v_lPMWrkTaskInstanceCnt)
                oDict.Add("v_lClaimDebtID", v_lClaimDebtID)
                oDict.Add("v_lInsuranceFileCnt", v_lInsuranceFileCnt)
                oDict.Add("v_lClaimCnt", v_lClaimCnt)
                oDict.Add("v_lCallingProcessKeyId", v_lCallingProcessKeyId)
                gPMFunctions.LogMessageToFile(sMsg:="Parameters add  Failed", vMethod:="GetEventTask", sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, vApp:=ACApp, vClass:=ACClass, excep:=New Exception(Information.Err().Description), oDicParms:=oDict)
            End If


            If .Parameters.Add("pmwrk_task_action_type_id", IIf(False, DBNull.Value, v_lClaimCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("v_lPMWrkTaskInstanceCnt", v_lPMWrkTaskInstanceCnt)
                oDict.Add("v_lClaimDebtID", v_lClaimDebtID)
                oDict.Add("v_lInsuranceFileCnt", v_lInsuranceFileCnt)
                oDict.Add("v_lClaimCnt", v_lClaimCnt)
                oDict.Add("v_lCallingProcessKeyId", v_lCallingProcessKeyId)
                gPMFunctions.LogMessageToFile(sMsg:="Parameters add  Failed", vMethod:="GetEventTask", sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, vApp:=ACApp, vClass:=ACClass, excep:=New Exception(Information.Err().Description), oDicParms:=oDict)
            End If


            If .Parameters.Add("pmwrk_task_action_type_id", IIf(False, DBNull.Value, v_lCallingProcessKeyId), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("v_lPMWrkTaskInstanceCnt", v_lPMWrkTaskInstanceCnt)
                oDict.Add("v_lClaimDebtID", v_lClaimDebtID)
                oDict.Add("v_lInsuranceFileCnt", v_lInsuranceFileCnt)
                oDict.Add("v_lClaimCnt", v_lClaimCnt)
                oDict.Add("v_lCallingProcessKeyId", v_lCallingProcessKeyId)
                gPMFunctions.LogMessageToFile(sMsg:="Parameters add  Failed", vMethod:="GetEventTask", sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, vApp:=ACApp, vClass:=ACClass, excep:=New Exception(Information.Err().Description), oDicParms:=oDict)
            End If


            If .Parameters.Add("pmwrk_task_action_type_id", IIf(False, DBNull.Value, v_lCallingProcessType), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("v_lPMWrkTaskInstanceCnt", v_lPMWrkTaskInstanceCnt)
                oDict.Add("v_lClaimDebtID", v_lClaimDebtID)
                oDict.Add("v_lInsuranceFileCnt", v_lInsuranceFileCnt)
                oDict.Add("v_lClaimCnt", v_lClaimCnt)
                oDict.Add("v_lCallingProcessKeyId", v_lCallingProcessKeyId)
                gPMFunctions.LogMessageToFile(sMsg:="Parameters add  Failed", vMethod:="GetEventTask", sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, vApp:=ACApp, vClass:=ACClass, excep:=New Exception(Information.Err().Description), oDicParms:=oDict)
            End If


            If .Parameters.Add("pmwrk_task_action_type_id", v_iUseWorkTables, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("v_lPMWrkTaskInstanceCnt", v_lPMWrkTaskInstanceCnt)
                oDict.Add("v_lClaimDebtID", v_lClaimDebtID)
                oDict.Add("v_lInsuranceFileCnt", v_lInsuranceFileCnt)
                oDict.Add("v_lClaimCnt", v_lClaimCnt)
                oDict.Add("v_lCallingProcessKeyId", v_lCallingProcessKeyId)
                gPMFunctions.LogMessageToFile(sMsg:="Parameters add  Failed", vMethod:="GetEventTask", sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, vApp:=ACApp, vClass:=ACClass, excep:=New Exception(Information.Err().Description), oDicParms:=oDict)
            End If


            If .SQLSelect(sSQL:=ACPMEvent_Task_FindSQL, sSQLName:=ACPMEvent_Task_FindName, bStoredProcedure:=ACPMEvent_Task_FindSP, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResults) <> gPMConstants.PMEReturnCode.PMTrue Then

                Throw New System.Exception(Constants.vbObjectError.ToString() + ", CheckTasks, " + "Error calling SQLSelect: " & ACPMEvent_Task_FindSQL)
            End If

            If Not (Information.IsArray(r_vResults)) Then
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If

        End With

        Return result
    End Function

    ' ***************************************************************** '
    ' Name: UpdateEventTask
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 07-07-2003 : workflow
    ' ***************************************************************** '
    Public Function UpdateEventTask(ByVal v_lEventCnt As Integer, ByVal v_iTaskStatus As Integer, ByVal v_lOutcomeId As Integer, ByVal v_vOutcomeDate As Object, Optional ByVal v_iUseWorkTables As Integer = gPMConstants.PMEReturnCode.PMFalse) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "UpdateEventTask"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters

            m_oDatabase.Parameters.Clear()

            ' Event Cnt
            m_lReturn = CType(AddInputParameter(v_sName:="Event_Cnt", v_vValue:=v_lEventCnt, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' Task Status
            m_lReturn = CType(AddInputParameter(v_sName:="Task_Status", v_vValue:=v_iTaskStatus, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' Task Outcome Id
            m_lReturn = CType(AddInputParameter(v_sName:="Task_Outcome_Id", v_vValue:=v_lOutcomeId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' Task Outcome Date

            m_lReturn = CType(AddInputParameter(v_sName:="Task_Outcome_Date", v_vValue:=CInt(v_vOutcomeDate), v_iType:=gPMConstants.PMEDataType.PMDate), gPMConstants.PMEReturnCode)

            ' UseWorkTables
            m_lReturn = CType(AddInputParameter(v_sName:="UseWorkTables", v_vValue:=v_iUseWorkTables, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' Execute Action Query

            If m_oDatabase.SQLAction(sSQL:=ACUpdateEventTaskSQL, sSQLName:=ACUpdateEventTaskName, bStoredProcedure:=True) <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse

                '******************************
                ' Log Error.
                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("v_lEventCnt", v_lEventCnt)
                oDict.Add("v_lOutcomeId", v_lOutcomeId)
                oDict.Add("v_vOutcomeDate", v_vOutcomeDate)
                gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to update specified event task", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=New Exception(Information.Err().Description), oDicParms:=oDict)
                '******************************

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error.
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_lEventCnt", v_lEventCnt)
            oDict.Add("v_lOutcomeId", v_lOutcomeId)
            oDict.Add("v_vOutcomeDate", v_vOutcomeDate)
            gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep, oDicParms:=oDict)
            '******************************

            Return result

        End Try
    End Function



























































    ' ***************************************************************** '
    ' Name: AddInputParameter
    '
    ' Parameters: v_sName   : Parameter Name
    '             v_vValue  : Parameter Value
    '             v_iType   : Parameter DataType
    '
    ' Description: Adds an input parameter to the database parameters
    '
    ' History:
    '           Created : MEvans : 18-12-2002 : 202
    ' ***************************************************************** '
    Private Function AddInputParameter(ByVal v_sName As String, ByVal v_vValue As Integer, ByVal v_iType As Integer, Optional ByRef v_iDirection As Integer = gPMConstants.PMEParameterDirection.PMParamInput) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "AddInputParameter"



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Add Parameter to database object

        If m_oDatabase.Parameters.Add(sName:=v_sName, vValue:=v_vValue, idirection:=v_iDirection, iDataType:=v_iType) <> gPMConstants.PMEReturnCode.PMTrue Then

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=New Exception(Information.Err().Description))

        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' STANDARD METHODS
    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByVal sUserName As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long Implements SSP.S4I.Interfaces.IBusiness.Initialise




        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            ' Set Username and Password
            m_sUsername = sUserName
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel



            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set Username and Password

            m_lCurrentRecord = 0
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            m_lReturn = CType(gPMComponentServices.CreateBusinessObject(r_oObject:=m_oLookup, v_sClassName:="bPMLookup.Business", v_sCallingAppName:="", v_sUsername:=sUserName, v_sPassword:=sPassword, v_iUserID:=iUserID, v_iSourceID:=iSourceID, v_iLanguageID:=iLanguageID, v_iCurrencyID:=iCurrencyID, v_iLogLevel:=iLogLevel, v_oDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_oLookup.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("iUserID", iUserID)
            oDict.Add("iSourceID", iSourceID)
            oDict.Add("iLanguageID", iLanguageID)
            oDict.Add("iCurrencyID", iCurrencyID)
            gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep, oDicParms:=oDict)

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

                m_lProcessMode = CType(CInt(vProcessMode), gPMConstants.PMEProcessMode)
            End If


            If Not Information.IsNothing(vTransactionType) Then

                m_sTransactionType = CStr(vTransactionType)
            End If


            If Not Information.IsNothing(vEffectiveDate) Then

                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("vEffectiveDate", vEffectiveDate)
            gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", excep:=excep, oDicParms:=oDict)

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
            If disposing Then
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                    m_oDatabase = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub



    ' ***************************************************************** '
    ' Name: BeginTrans (Private)
    '
    ' Description: Begins a Transaction.
    '
    ' ***************************************************************** '
    Public Function BeginTrans() As Integer

        Dim result As Integer = 0
        'Const sFunctionName As String = "BeginTrans"       ''Unused Local Variable

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction

            m_lReturn = m_oDatabase.SQLBeginTrans

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Terminate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Terminate", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CommitTrans (Private)
    '
    ' Description: Commits a Transaction (Saves changes to DB).
    '
    ' ***************************************************************** '
    Public Function CommitTrans() As Integer

        Dim result As Integer = 0
        'Const sFunctionName As String = "CommitTrans"      ''Unused Local Variables

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction

            m_lReturn = m_oDatabase.SQLCommitTrans

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Terminate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Terminate", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: RollbackTrans (Private)
    '
    ' Description: Rollback a Transaction (Undo changes to DB).
    '
    ' ***************************************************************** '
    Public Function RollbackTrans() As Integer

        Dim result As Integer = 0
        ''Const sFunctionName As String = "RollbackTrans"       ''Unused Local Variable

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction

            m_lReturn = m_oDatabase.SQLRollbackTrans

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Terminate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Terminate", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' END STANDARD METHODS
    ' ***************************************************************** '
End Class

