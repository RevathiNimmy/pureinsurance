Option Strict Off
Option Explicit On
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
    ' Added to replace global variables 06/10/2003
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

    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        End Get
    End Property

    ' ***************************************************************** '
    ' Name: GetMaintainData
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 25-06-2003 : workflow
    ' ***************************************************************** '
    Public Function GetMaintainData(ByVal v_lWorkflowId As Integer, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "GetMaintainData"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters

            m_oDatabase.Parameters.Clear()

            ' workflow id
            m_lReturn = CType(AddInputParameter(v_sName:="workflow_id", v_vValue:=v_lWorkflowId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' Execute selection Query

            If m_oDatabase.SQLSelect(sSQL:=ACGetMaintainDataSQL, sSQLName:=ACGetMaintainDataName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse

                '******************************
                ' Log Error.
                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("v_lWorkflowId", v_lWorkflowId)
                gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=New Exception(Information.Err().Description), oDicParms:=oDict)
                '******************************

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error.
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_lWorkflowId", v_lWorkflowId)
            gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep, oDicParms:=oDict)
            '******************************


            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: AddPackageStep
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 03-10-2003 : 229
    ' ***************************************************************** '
    Public Function AddPackageStep(ByRef r_lWorkflowStepId As Integer, ByVal v_lWorkflowId As Integer, ByVal v_lStepOrder As Integer, ByVal v_sStepCode As String, ByVal v_sStepDescription As String, ByVal v_dtEffectiveDate As Date, ByVal v_bIsDeleted As Boolean, ByVal v_lTaskGroupId As Integer, ByVal v_lTaskId As Integer, ByVal v_lTaskActionTypeId As Integer, ByVal v_lPMUserGroupID As Integer, ByVal v_lUserId As Integer, ByVal v_lStepDaysDuration As Object, ByVal v_lCompleteNextWorkflowStepId As Integer, ByVal v_lOverdueNextWorkflowstepId As Integer, ByVal v_bExecuatableTask As Boolean, ByVal v_lEventTypeId As Integer, ByVal v_lEventLogSubjectId As Integer, ByVal v_sEventDescription As String, ByVal v_sTaskDescription As String, ByVal v_bIsUrgent As Boolean, ByVal v_sCustomer As String, ByVal v_sWorkflow As String, ByVal v_lBranchId As Integer) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "AddPackageStep"

        Try

            'caption_id, done in stored procedure....

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters

            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters

            '**************
            ' OUTPUT PARAMETER
            ' workflow step id
            m_lReturn = CType(AddInputParameter(v_sName:="pmwrk_workflow_step_id", v_vValue:=r_lWorkflowStepId, v_iType:=gPMConstants.PMEDataType.PMLong, v_iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput), gPMConstants.PMEReturnCode)
            '**************

            ' workflow id
            m_lReturn = CType(AddInputParameter(v_sName:="pmwrk_workflow_id", v_vValue:=v_lWorkflowId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' step order
            m_lReturn = CType(AddInputParameter(v_sName:="step_order", v_vValue:=v_lStepOrder, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' step code
            m_lReturn = CType(AddInputParameter(v_sName:="step_code", v_vValue:=v_sStepCode, v_iType:=gPMConstants.PMEDataType.PMString), gPMConstants.PMEReturnCode)

            ' step description
            m_lReturn = CType(AddInputParameter(v_sName:="step_description", v_vValue:=v_sStepDescription, v_iType:=gPMConstants.PMEDataType.PMString), gPMConstants.PMEReturnCode)

            ' effective date
            m_lReturn = CType(AddInputParameter(v_sName:="effective_date", v_vValue:=v_dtEffectiveDate, v_iType:=gPMConstants.PMEDataType.PMDate), gPMConstants.PMEReturnCode)

            ' is_deleted
            If v_bIsDeleted Then
                m_lReturn = CType(AddInputParameter(v_sName:="is_deleted", v_vValue:=1, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            Else
                m_lReturn = CType(AddInputParameter(v_sName:="is_deleted", v_vValue:=0, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            End If

            ' task group id
            m_lReturn = CType(AddInputParameter(v_sName:="task_group_id", v_vValue:=v_lTaskGroupId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' task id
            m_lReturn = CType(AddInputParameter(v_sName:="task_id", v_vValue:=v_lTaskId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' pmuser group id
            m_lReturn = CType(AddInputParameter(v_sName:="pmuser_group_id", v_vValue:=v_lPMUserGroupID, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' user id
            If v_lUserId < 1 Then

                m_lReturn = CType(AddInputParameter(v_sName:="user_id", v_vValue:=DBNull.Value, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            Else
                m_lReturn = CType(AddInputParameter(v_sName:="user_id", v_vValue:=v_lUserId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            End If

            ' step days duration
            m_lReturn = CType(AddInputParameter(v_sName:="step_days_duration", v_vValue:=v_lStepDaysDuration, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            If v_lCompleteNextWorkflowStepId < 1 Then
                ' complete next step id

                m_lReturn = CType(AddInputParameter(v_sName:="complete_next_workflow_step_id", v_vValue:=DBNull.Value, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            Else
                m_lReturn = CType(AddInputParameter(v_sName:="complete_next_workflow_step_id", v_vValue:=v_lCompleteNextWorkflowStepId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            End If

            If v_lOverdueNextWorkflowstepId < 1 Then
                ' overdue next step id

                m_lReturn = CType(AddInputParameter(v_sName:="overdue_next_workflow_step_id", v_vValue:=DBNull.Value, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            Else
                m_lReturn = CType(AddInputParameter(v_sName:="overdue_next_workflow_step_id", v_vValue:=v_lOverdueNextWorkflowstepId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            End If

            ' executable task
            If v_bExecuatableTask Then
                m_lReturn = CType(AddInputParameter(v_sName:="Executable_Task", v_vValue:=1, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            Else
                m_lReturn = CType(AddInputParameter(v_sName:="Executable_Task", v_vValue:=0, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            End If

            ' task action type id
            If v_lTaskActionTypeId < 1 Then

                m_lReturn = CType(AddInputParameter(v_sName:="PMWrk_Task_Action_type_id", v_vValue:=DBNull.Value, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            Else
                m_lReturn = CType(AddInputParameter(v_sName:="PMWrk_Task_Action_type_id", v_vValue:=v_lTaskActionTypeId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            End If

            ' event type id
            If v_lEventTypeId < 1 Then

                m_lReturn = CType(AddInputParameter(v_sName:="Event_Type_Id", v_vValue:=DBNull.Value, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            Else
                m_lReturn = CType(AddInputParameter(v_sName:="Event_Type_Id", v_vValue:=v_lEventTypeId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            End If

            ' event description
            If v_sEventDescription = "" Then

                m_lReturn = CType(AddInputParameter(v_sName:="Event_Description", v_vValue:=DBNull.Value, v_iType:=gPMConstants.PMEDataType.PMString), gPMConstants.PMEReturnCode)
            Else
                m_lReturn = CType(AddInputParameter(v_sName:="Event_Description", v_vValue:=v_sEventDescription, v_iType:=gPMConstants.PMEDataType.PMString), gPMConstants.PMEReturnCode)
            End If

            ' event log subject id
            If v_lEventLogSubjectId < 1 Then

                m_lReturn = CType(AddInputParameter(v_sName:="Event_Log_Subject_Id", v_vValue:=DBNull.Value, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            Else
                m_lReturn = CType(AddInputParameter(v_sName:="Event_Log_Subject_Id", v_vValue:=v_lEventLogSubjectId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            End If

            If v_sTaskDescription = "" Then

                m_lReturn = CType(AddInputParameter(v_sName:="Task_Description", v_vValue:=DBNull.Value, v_iType:=gPMConstants.PMEDataType.PMString), gPMConstants.PMEReturnCode)
            Else
                m_lReturn = CType(AddInputParameter(v_sName:="Task_Description", v_vValue:=v_sTaskDescription, v_iType:=gPMConstants.PMEDataType.PMString), gPMConstants.PMEReturnCode)
            End If

            ' IsUrgent
            If v_bIsUrgent Then
                m_lReturn = CType(AddInputParameter(v_sName:="IsUrgent", v_vValue:=1, v_iType:=gPMConstants.PMEDataType.PMBoolean), gPMConstants.PMEReturnCode)
            Else
                m_lReturn = CType(AddInputParameter(v_sName:="IsUrgent", v_vValue:=0, v_iType:=gPMConstants.PMEDataType.PMBoolean), gPMConstants.PMEReturnCode)
            End If

            ' Customer
            If v_sCustomer = "" Then

                m_lReturn = CType(AddInputParameter(v_sName:="Customer", v_vValue:=DBNull.Value, v_iType:=gPMConstants.PMEDataType.PMString), gPMConstants.PMEReturnCode)
            Else
                m_lReturn = CType(AddInputParameter(v_sName:="Customer", v_vValue:=v_sCustomer, v_iType:=gPMConstants.PMEDataType.PMString), gPMConstants.PMEReturnCode)
            End If

            ' Workflow
            If v_sWorkflow = "" Then

                m_lReturn = CType(AddInputParameter(v_sName:="Workflow", v_vValue:=DBNull.Value, v_iType:=gPMConstants.PMEDataType.PMString), gPMConstants.PMEReturnCode)
            Else
                m_lReturn = CType(AddInputParameter(v_sName:="Workflow", v_vValue:=v_sWorkflow, v_iType:=gPMConstants.PMEDataType.PMString), gPMConstants.PMEReturnCode)
            End If

            ' branch_id
            m_lReturn = CType(AddInputParameter(v_sName:="Source_Id", v_vValue:=v_lBranchId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)


            ' Execute Action Query

            If m_oDatabase.SQLAction(sSQL:=ACAddPackageStepSQL, sSQLName:=ACAddPackageStepName, bStoredProcedure:=True) <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse

                '******************************
                ' Log Error.
                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("r_lWorkflowStepId", r_lWorkflowStepId)
                oDict.Add("v_lWorkflowId", v_lWorkflowId)
                oDict.Add("v_sStepCode", v_sStepCode)
                oDict.Add("v_dtEffectiveDate", v_dtEffectiveDate)
                oDict.Add("v_lTaskGroupId", v_lTaskGroupId)
                oDict.Add("v_lTaskId", v_lTaskId)
                oDict.Add("v_lTaskActionTypeId", v_lTaskActionTypeId)
                oDict.Add("v_lPMUserGroupID", v_lPMUserGroupID)
                oDict.Add("v_lUserId", v_lUserId)
                oDict.Add("v_lCompleteNextWorkflowStepId", v_lCompleteNextWorkflowStepId)
                oDict.Add("v_lOverdueNextWorkflowstepId", v_lOverdueNextWorkflowstepId)
                oDict.Add("v_lEventTypeId", v_lEventTypeId)
                oDict.Add("v_lEventLogSubjectId", v_lEventLogSubjectId)
                oDict.Add("v_lBranchId", v_lBranchId)
                gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=New Exception(Information.Err().Description), oDicParms:=oDict)
                '******************************

            Else
                ' get the steps id..

                r_lWorkflowStepId = m_oDatabase.Parameters.Item("pmwrk_workflow_step_id").Value
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error.
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("r_lWorkflowStepId", r_lWorkflowStepId)
            oDict.Add("v_lWorkflowId", v_lWorkflowId)
            oDict.Add("v_sStepCode", v_sStepCode)
            oDict.Add("v_dtEffectiveDate", v_dtEffectiveDate)
            oDict.Add("v_lTaskGroupId", v_lTaskGroupId)
            oDict.Add("v_lTaskId", v_lTaskId)
            oDict.Add("v_lTaskActionTypeId", v_lTaskActionTypeId)
            oDict.Add("v_lPMUserGroupID", v_lPMUserGroupID)
            oDict.Add("v_lUserId", v_lUserId)
            oDict.Add("v_lCompleteNextWorkflowStepId", v_lCompleteNextWorkflowStepId)
            oDict.Add("v_lOverdueNextWorkflowstepId", v_lOverdueNextWorkflowstepId)
            oDict.Add("v_lEventTypeId", v_lEventTypeId)
            oDict.Add("v_lEventLogSubjectId", v_lEventLogSubjectId)
            oDict.Add("v_lBranchId", v_lBranchId)
            gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep, oDicParms:=oDict)
            '******************************

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: UpdatePackageStep
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 03-10-2003 : 229
    ' ***************************************************************** '
    Public Function UpdatePackageStep(ByVal v_lWorkflowStepId As Integer, ByVal v_lWorkflowId As Integer, ByVal v_lStepOrder As Integer, ByVal v_sStepCode As String, ByVal v_sStepDescription As String, ByVal v_dtEffectiveDate As Date, ByVal v_bIsDeleted As Boolean, ByVal v_lTaskGroupId As Integer, ByVal v_lTaskId As Integer, ByVal v_lTaskActionTypeId As Integer, ByVal v_lPMUserGroupID As Integer, ByVal v_lUserId As Integer, ByVal v_lStepDaysDuration As Object, ByVal v_lCompleteNextWorkflowStepId As Integer, ByVal v_lOverdueNextWorkflowstepId As Integer, ByVal v_bExecuatableTask As Boolean, ByVal v_lEventTypeId As Integer, ByVal v_lEventLogSubjectId As Integer, ByVal v_sEventDescription As String, ByVal v_sTaskDescription As String, ByVal v_bIsUrgent As Boolean, ByVal v_sCustomer As String, ByVal v_sWorkflow As String, ByVal v_lBranchId As Integer) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "UpdatePackageStep"

        Try

            'caption_id, done in stored procedure....

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters

            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters

            ' workflow step id
            m_lReturn = CType(AddInputParameter(v_sName:="pmwrk_workflow_step_id", v_vValue:=v_lWorkflowStepId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' workflow id
            m_lReturn = CType(AddInputParameter(v_sName:="pmwrk_workflow_id", v_vValue:=v_lWorkflowId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' step order
            m_lReturn = CType(AddInputParameter(v_sName:="step_order", v_vValue:=v_lStepOrder, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' step code
            m_lReturn = CType(AddInputParameter(v_sName:="step_code", v_vValue:=v_sStepCode, v_iType:=gPMConstants.PMEDataType.PMString), gPMConstants.PMEReturnCode)

            ' step description
            m_lReturn = CType(AddInputParameter(v_sName:="step_description", v_vValue:=v_sStepDescription, v_iType:=gPMConstants.PMEDataType.PMString), gPMConstants.PMEReturnCode)

            ' effective date
            m_lReturn = CType(AddInputParameter(v_sName:="effective_date", v_vValue:=v_dtEffectiveDate, v_iType:=gPMConstants.PMEDataType.PMDate), gPMConstants.PMEReturnCode)

            ' is_deleted
            If v_bIsDeleted Then
                m_lReturn = CType(AddInputParameter(v_sName:="is_deleted", v_vValue:=1, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            Else
                m_lReturn = CType(AddInputParameter(v_sName:="is_deleted", v_vValue:=0, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            End If

            ' task group id
            m_lReturn = CType(AddInputParameter(v_sName:="task_group_id", v_vValue:=v_lTaskGroupId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' task id
            m_lReturn = CType(AddInputParameter(v_sName:="task_id", v_vValue:=v_lTaskId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' pmuser group id
            m_lReturn = CType(AddInputParameter(v_sName:="pmuser_group_id", v_vValue:=v_lPMUserGroupID, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' user id
            If v_lUserId < 1 Then

                m_lReturn = CType(AddInputParameter(v_sName:="user_id", v_vValue:=DBNull.Value, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            Else
                m_lReturn = CType(AddInputParameter(v_sName:="user_id", v_vValue:=v_lUserId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            End If

            ' step days duration
            m_lReturn = CType(AddInputParameter(v_sName:="step_days_duration", v_vValue:=v_lStepDaysDuration, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            If v_lCompleteNextWorkflowStepId = 0 Then
                ' complete next step id

                m_lReturn = CType(AddInputParameter(v_sName:="complete_next_workflow_step_id", v_vValue:=DBNull.Value, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            Else
                m_lReturn = CType(AddInputParameter(v_sName:="complete_next_workflow_step_id", v_vValue:=v_lCompleteNextWorkflowStepId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            End If

            If v_lOverdueNextWorkflowstepId = 0 Then
                ' overdue next step id

                m_lReturn = CType(AddInputParameter(v_sName:="overdue_next_workflow_step_id", v_vValue:=DBNull.Value, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            Else
                m_lReturn = CType(AddInputParameter(v_sName:="overdue_next_workflow_step_id", v_vValue:=v_lOverdueNextWorkflowstepId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            End If

            ' executable task
            If v_bExecuatableTask Then
                m_lReturn = CType(AddInputParameter(v_sName:="Executable_Task", v_vValue:=1, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            Else
                m_lReturn = CType(AddInputParameter(v_sName:="Executable_Task", v_vValue:=0, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            End If

            ' task action type id
            If v_lTaskActionTypeId < 1 Then

                m_lReturn = CType(AddInputParameter(v_sName:="PMWrk_Task_Action_type_id", v_vValue:=DBNull.Value, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            Else
                m_lReturn = CType(AddInputParameter(v_sName:="PMWrk_Task_Action_type_id", v_vValue:=v_lTaskActionTypeId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            End If

            ' event type id
            If v_lEventTypeId < 1 Then

                m_lReturn = CType(AddInputParameter(v_sName:="Event_Type_Id", v_vValue:=DBNull.Value, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            Else
                m_lReturn = CType(AddInputParameter(v_sName:="Event_Type_Id", v_vValue:=v_lEventTypeId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            End If

            ' event description
            If v_sEventDescription = "" Then

                m_lReturn = CType(AddInputParameter(v_sName:="Event_Description", v_vValue:=DBNull.Value, v_iType:=gPMConstants.PMEDataType.PMString), gPMConstants.PMEReturnCode)
            Else
                m_lReturn = CType(AddInputParameter(v_sName:="Event_Description", v_vValue:=v_sEventDescription, v_iType:=gPMConstants.PMEDataType.PMString), gPMConstants.PMEReturnCode)
            End If

            ' event log subject id
            If v_lEventLogSubjectId < 1 Then

                m_lReturn = CType(AddInputParameter(v_sName:="Event_Log_Subject_Id", v_vValue:=DBNull.Value, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            Else
                m_lReturn = CType(AddInputParameter(v_sName:="Event_Log_Subject_Id", v_vValue:=v_lEventLogSubjectId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            End If

            ' task description
            If v_sTaskDescription = "" Then

                m_lReturn = CType(AddInputParameter(v_sName:="Task_Description", v_vValue:=DBNull.Value, v_iType:=gPMConstants.PMEDataType.PMString), gPMConstants.PMEReturnCode)
            Else
                m_lReturn = CType(AddInputParameter(v_sName:="Task_Description", v_vValue:=v_sTaskDescription, v_iType:=gPMConstants.PMEDataType.PMString), gPMConstants.PMEReturnCode)
            End If

            ' IsUrgent
            If v_bIsUrgent Then
                m_lReturn = CType(AddInputParameter(v_sName:="IsUrgent", v_vValue:=1, v_iType:=gPMConstants.PMEDataType.PMBoolean), gPMConstants.PMEReturnCode)
            Else
                m_lReturn = CType(AddInputParameter(v_sName:="IsUrgent", v_vValue:=0, v_iType:=gPMConstants.PMEDataType.PMBoolean), gPMConstants.PMEReturnCode)
            End If

            ' Customer
            If v_sCustomer = "" Then

                m_lReturn = CType(AddInputParameter(v_sName:="Customer", v_vValue:=DBNull.Value, v_iType:=gPMConstants.PMEDataType.PMString), gPMConstants.PMEReturnCode)
            Else
                m_lReturn = CType(AddInputParameter(v_sName:="Customer", v_vValue:=v_sCustomer, v_iType:=gPMConstants.PMEDataType.PMString), gPMConstants.PMEReturnCode)
            End If

            ' Workflow
            If v_sWorkflow = "" Then

                m_lReturn = CType(AddInputParameter(v_sName:="Workflow", v_vValue:=DBNull.Value, v_iType:=gPMConstants.PMEDataType.PMString), gPMConstants.PMEReturnCode)
            Else
                m_lReturn = CType(AddInputParameter(v_sName:="Workflow", v_vValue:=v_sWorkflow, v_iType:=gPMConstants.PMEDataType.PMString), gPMConstants.PMEReturnCode)
            End If

            ' branch id
            m_lReturn = CType(AddInputParameter(v_sName:="Source_Id", v_vValue:=v_lBranchId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)


            ' Execute Action Query

            If m_oDatabase.SQLAction(sSQL:=ACUpdatePackageStepSQL, sSQLName:=ACUpdatePackageStepName, bStoredProcedure:=True) <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse

                '******************************
                ' Log Error.
                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("v_lWorkflowStepId", v_lWorkflowStepId)
                oDict.Add("v_lWorkflowId", v_lWorkflowId)
                oDict.Add("v_sStepCode", v_sStepCode)
                oDict.Add("v_dtEffectiveDate", v_dtEffectiveDate)
                oDict.Add("v_lTaskGroupId", v_lTaskGroupId)
                oDict.Add("v_lTaskId", v_lTaskId)
                oDict.Add("v_lTaskActionTypeId", v_lTaskActionTypeId)
                oDict.Add("v_lPMUserGroupID", v_lPMUserGroupID)
                oDict.Add("v_lUserId", v_lUserId)
                oDict.Add("v_lCompleteNextWorkflowStepId", v_lCompleteNextWorkflowStepId)
                oDict.Add("v_lOverdueNextWorkflowstepId", v_lOverdueNextWorkflowstepId)
                oDict.Add("v_lEventTypeId", v_lEventTypeId)
                oDict.Add("v_lEventLogSubjectId", v_lEventLogSubjectId)
                oDict.Add("v_lBranchId", v_lBranchId)
                gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=New Exception(Information.Err().Description), oDicParms:=oDict)
                '******************************

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error.
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_lWorkflowStepId", v_lWorkflowStepId)
            oDict.Add("v_lWorkflowId", v_lWorkflowId)
            oDict.Add("v_sStepCode", v_sStepCode)
            oDict.Add("v_dtEffectiveDate", v_dtEffectiveDate)
            oDict.Add("v_lTaskGroupId", v_lTaskGroupId)
            oDict.Add("v_lTaskId", v_lTaskId)
            oDict.Add("v_lTaskActionTypeId", v_lTaskActionTypeId)
            oDict.Add("v_lPMUserGroupID", v_lPMUserGroupID)
            oDict.Add("v_lUserId", v_lUserId)
            oDict.Add("v_lCompleteNextWorkflowStepId", v_lCompleteNextWorkflowStepId)
            oDict.Add("v_lOverdueNextWorkflowstepId", v_lOverdueNextWorkflowstepId)
            oDict.Add("v_lEventTypeId", v_lEventTypeId)
            oDict.Add("v_lEventLogSubjectId", v_lEventLogSubjectId)
            oDict.Add("v_lBranchId", v_lBranchId)
            gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep, oDicParms:=oDict)
            '******************************

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetValidUserBranches
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 20-10-2003 : Continuation Tasks
    ' ***************************************************************** '
    Public Function GetValidUserBranches(ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "GetValidUserBranches"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters

            m_oDatabase.Parameters.Clear()

            ' Execute selection Query

            If m_oDatabase.SQLSelect(sSQL:=ACGetValidUserBranchesSQL, sSQLName:=ACGetValidUserBranchesName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse

                '******************************
                ' Log Error.
                gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to retrieve valid user branches", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=New Exception(Information.Err().Description))
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
    Private Function AddInputParameter(ByVal v_sName As String, ByVal v_vValue As Object, ByVal v_iType As Integer, Optional ByRef v_iDirection As Integer = gPMConstants.PMEParameterDirection.PMParamInput) As Integer

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
        Const sFunctionName As String = "BeginTrans"

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
        Const sFunctionName As String = "CommitTrans"

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
        Const sFunctionName As String = "RollbackTrans"

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

