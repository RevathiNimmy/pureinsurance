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


    ' ************************************************
    ' Added to replace global variables 01/10/2003
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

    Private m_vTaskKeys As Object
    Private m_lTaskInstKeysTaskCnt As Integer
    Private m_oDocManagerWrapper As bSIRDocManagerWrapper.Interface_Renamed
    Private m_oFindDocTemplate As Object
    Private m_oLookup As bPMLookup.Business


    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        End Get
    End Property

    ' ***************************************************************** '
    ' Name: SetupWorkWorkflow
    '
    ' Parameters:
    '       v_lPMWrkTaskInstanceCnt: is from work_pmwrk_task_instance
    '
    ' Description: Creates the initial work step used to kick off
    '               the workflow package continuation tasks
    '
    ' History:
    '           Created : MEvans : 07-10-2003 : Dev - Continuation tasks
    ' ***************************************************************** '
    Public Function SetupWorkWorkflow(ByVal v_lWorkflowPackageId As Integer, ByVal v_lCallingProcessKeyId As Integer, ByVal v_lCallingProcessTypeID As Integer, ByVal v_lPMWrkTaskInstanceCnt As Integer, ByVal v_lStartStepCnt As Integer, Optional ByVal v_vStepGroupUsers As Object = Nothing, Optional ByVal v_vStepData As Object = Nothing) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "SetupWorkWorkflow"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create Step Instance based on first step or provided step and task instance cnt
            If CreateWorkStepInstance(v_lWorkflowStepId:=v_lStartStepCnt, v_lTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt, v_lCallingProcessKeyId:=v_lCallingProcessKeyId, v_lCallingProcessTypeID:=v_lCallingProcessTypeID) = gPMConstants.PMEReturnCode.PMTrue Then

                ' Create Step Group Users from array

                If ProcessStepGroupUser(v_lCallingProcessKeyId:=v_lCallingProcessKeyId, v_lCallingProcessTypeID:=v_lCallingProcessTypeID, v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt, v_lStartStepCnt:=v_lStartStepCnt, v_vStepGroupUsers:=v_vStepGroupUsers) = gPMConstants.PMEReturnCode.PMTrue Then


                    ' Create Step Data from array

                    If ProcessStepData(v_lCallingProcessKeyId:=v_lCallingProcessKeyId, v_lCallingProcessTypeID:=v_lCallingProcessTypeID, v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt, v_lStartStepCnt:=v_lStartStepCnt, v_vStepData:=v_vStepData) <> gPMConstants.PMEReturnCode.PMTrue Then

                        result = gPMConstants.PMEReturnCode.PMFalse

                        Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                        oDict.Add("v_lWorkflowPackageId", v_lWorkflowPackageId)
                        oDict.Add("v_lCallingProcessKeyId", v_lCallingProcessKeyId)
                        oDict.Add("v_lCallingProcessTypeID", v_lCallingProcessTypeID)
                        oDict.Add("v_lPMWrkTaskInstanceCnt", v_lPMWrkTaskInstanceCnt)
                        oDict.Add("v_lStartStepCnt", v_lStartStepCnt)
                        gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to process work step data for package" & CStr(v_lWorkflowPackageId), vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, oDicParms:=oDict)

                    End If

                Else

                    result = gPMConstants.PMEReturnCode.PMFalse

                    Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                    oDict.Add("v_lWorkflowPackageId", v_lWorkflowPackageId)
                    oDict.Add("v_lCallingProcessKeyId", v_lCallingProcessKeyId)
                    oDict.Add("v_lCallingProcessTypeID", v_lCallingProcessTypeID)
                    oDict.Add("v_lPMWrkTaskInstanceCnt", v_lPMWrkTaskInstanceCnt)
                    oDict.Add("v_lStartStepCnt", v_lStartStepCnt)
                    gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to process work step group users for package" & CStr(v_lWorkflowPackageId), vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, oDicParms:=oDict)

                End If

            Else

                result = gPMConstants.PMEReturnCode.PMFalse

                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("v_lWorkflowPackageId", v_lWorkflowPackageId)
                oDict.Add("v_lCallingProcessKeyId", v_lCallingProcessKeyId)
                oDict.Add("v_lCallingProcessTypeID", v_lCallingProcessTypeID)
                oDict.Add("v_lPMWrkTaskInstanceCnt", v_lPMWrkTaskInstanceCnt)
                oDict.Add("v_lStartStepCnt", v_lStartStepCnt)
                gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to create work step instance for step:" & CStr(v_lStartStepCnt), vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, oDicParms:=oDict)

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error.
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_lWorkflowPackageId", v_lWorkflowPackageId)
            oDict.Add("v_lCallingProcessKeyId", v_lCallingProcessKeyId)
            oDict.Add("v_lCallingProcessTypeID", v_lCallingProcessTypeID)
            oDict.Add("v_lPMWrkTaskInstanceCnt", v_lPMWrkTaskInstanceCnt)
            oDict.Add("v_lStartStepCnt", v_lStartStepCnt)
            gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep, oDicParms:=oDict)
            '******************************

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ProcessNextStep
    '
    ' Parameters: n/a
    '
    ' Description: Performs all necessary work to create action the
    '               next necessary step..
    '
    ' History:
    '           Created : MEvans : 08-10-2003 : continuation tasks
    ' ***************************************************************** '
    Public Function ProcessNextStep(ByVal v_lStepInstanceCnt As Integer, ByVal v_lTaskInstanceCnt As Integer, ByVal v_lNextStepId As Byte, Optional ByVal v_lWorkflowPackageId As Integer = 0) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "ProcessNextStep"

        Dim vStepDetails As Object
        Dim lTaskInstanceCnt, lEventCnt As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' if this is a valid next step.
            If v_lNextStepId <> 0 Then

                ' get the task instances keys if they havent been returned already
                If GetTaskInstKeys(v_lTaskInstanceCnt:=v_lTaskInstanceCnt) = gPMConstants.PMEReturnCode.PMTrue Then

                    ' update step instance to point to the new step
                    If UpdateStepInstance(v_lStepId:=v_lNextStepId, v_lTaskInstanceCnt:=v_lTaskInstanceCnt, v_lStepInstanceCnt:=v_lStepInstanceCnt) = gPMConstants.PMEReturnCode.PMTrue Then

                        ' get enough details so that the event task can be created.
                        If GetStepDetails(v_lStepInstanceCnt:=v_lStepInstanceCnt, r_vResults:=vStepDetails) = gPMConstants.PMEReturnCode.PMTrue Then

                            ' create a new event task
                            ' NB: This also does the copy keys...

                            If CreateNewEventTask(r_lTaskInstanceCnt:=lTaskInstanceCnt, r_lEventCnt:=lEventCnt, v_vStepDetails:=vStepDetails) = gPMConstants.PMEReturnCode.PMTrue Then

                                ' update step instance to point to the new task instance
                                If UpdateStepInstance(v_lStepId:=v_lNextStepId, v_lTaskInstanceCnt:=lTaskInstanceCnt, v_lStepInstanceCnt:=v_lStepInstanceCnt) = gPMConstants.PMEReturnCode.PMTrue Then

                                    ' Process the step
                                    If ProcessStep(v_lStepInstanceCnt:=v_lStepInstanceCnt, v_lTaskInstanceCnt:=lTaskInstanceCnt, v_lStepId:=v_lNextStepId) <> gPMConstants.PMEReturnCode.PMTrue Then

                                        result = gPMConstants.PMEReturnCode.PMError

                                        ' Log Error.
                                        Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                                        oDict.Add("v_lStepInstanceCnt", v_lStepInstanceCnt)
                                        oDict.Add("v_lTaskInstanceCnt", v_lTaskInstanceCnt)
                                        oDict.Add("v_lNextStepId", v_lNextStepId)
                                        oDict.Add("v_lWorkflowPackageId", v_lWorkflowPackageId)
                                        gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to Processtep", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, oDicParms:=oDict)

                                    End If

                                Else

                                    result = gPMConstants.PMEReturnCode.PMError

                                    ' Log Error.
                                    Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                                    oDict.Add("v_lStepInstanceCnt", v_lStepInstanceCnt)
                                    oDict.Add("v_lTaskInstanceCnt", v_lTaskInstanceCnt)
                                    oDict.Add("v_lNextStepId", v_lNextStepId)
                                    oDict.Add("v_lWorkflowPackageId", v_lWorkflowPackageId)
                                    gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to UpdateStepInstance with new task instance cnt", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, oDicParms:=oDict)

                                End If

                            Else

                                result = gPMConstants.PMEReturnCode.PMError

                                ' Log Error.
                                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                                oDict.Add("v_lStepInstanceCnt", v_lStepInstanceCnt)
                                oDict.Add("v_lTaskInstanceCnt", v_lTaskInstanceCnt)
                                oDict.Add("v_lNextStepId", v_lNextStepId)
                                oDict.Add("v_lWorkflowPackageId", v_lWorkflowPackageId)
                                gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to create new event task", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, oDicParms:=oDict)

                            End If

                        Else

                            result = gPMConstants.PMEReturnCode.PMError

                            ' Log Error.
                            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                            oDict.Add("v_lStepInstanceCnt", v_lStepInstanceCnt)
                            oDict.Add("v_lTaskInstanceCnt", v_lTaskInstanceCnt)
                            oDict.Add("v_lNextStepId", v_lNextStepId)
                            oDict.Add("v_lWorkflowPackageId", v_lWorkflowPackageId)
                            gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to getstepdetails", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, oDicParms:=oDict)

                        End If

                    Else

                        result = gPMConstants.PMEReturnCode.PMError

                        ' Log Error.
                        Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                        oDict.Add("v_lStepInstanceCnt", v_lStepInstanceCnt)
                        oDict.Add("v_lTaskInstanceCnt", v_lTaskInstanceCnt)
                        oDict.Add("v_lNextStepId", v_lNextStepId)
                        oDict.Add("v_lWorkflowPackageId", v_lWorkflowPackageId)
                        gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to update step instance", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, oDicParms:=oDict)

                    End If

                Else

                    result = gPMConstants.PMEReturnCode.PMError

                    ' Log Error.
                    Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                    oDict.Add("v_lStepInstanceCnt", v_lStepInstanceCnt)
                    oDict.Add("v_lTaskInstanceCnt", v_lTaskInstanceCnt)
                    oDict.Add("v_lNextStepId", v_lNextStepId)
                    oDict.Add("v_lWorkflowPackageId", v_lWorkflowPackageId)
                    gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to retrieve task instance keys for task instance:" & CStr(v_lTaskInstanceCnt), vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, oDicParms:=oDict)

                End If

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error.
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_lStepInstanceCnt", v_lStepInstanceCnt)
            oDict.Add("v_lTaskInstanceCnt", v_lTaskInstanceCnt)
            oDict.Add("v_lNextStepId", v_lNextStepId)
            oDict.Add("v_lWorkflowPackageId", v_lWorkflowPackageId)
            gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep, oDicParms:=oDict)
            '******************************

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CreateWorkStepData
    '
    ' Parameters:   v_vClaimPartyId     - can be null
    '               v_vClaimDebtId      - can be null
    '               v_vTaskDescription  - can be null
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 08-10-2003 : continuation tasks
    ' ***************************************************************** '
    Private Function CreateWorkStepData(ByVal v_lTaskInstanceCnt As Integer, ByVal v_lWorkflowStepId As Integer, ByVal v_vClaimPartyId As Object, ByVal v_vClaimDebtId As Object, ByVal v_vTaskDescription As Object, ByVal v_vTaskDuration As Object, ByVal v_lCallingProcessKeyId As Integer, ByVal v_lCallingProcessTypeID As Integer) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "CreateWorkStepData"



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear Down Database Parameters
        m_oDatabase.Parameters.Clear()

        ' Add Required Stored Procedure Parameters

        ' task instance cnt
        m_lReturn = CType(AddInputParameter(v_sName:="pmwrk_task_instance_cnt", v_vValue:=v_lTaskInstanceCnt, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

        ' workflow step id
        m_lReturn = CType(AddInputParameter(v_sName:="pmwrk_workflow_step_id", v_vValue:=v_lWorkflowStepId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

        ' claim party id - can be null
        m_lReturn = CType(AddInputParameter(v_sName:="claim_party_id", v_vValue:=v_vClaimPartyId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

        ' claim debt id - can be null
        m_lReturn = CType(AddInputParameter(v_sName:="claim_debt_id", v_vValue:=v_vClaimDebtId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

        ' task description - can be null
        m_lReturn = CType(AddInputParameter(v_sName:="task_description", v_vValue:=v_vTaskDescription, v_iType:=gPMConstants.PMEDataType.PMString), gPMConstants.PMEReturnCode)

        ' task duration - can be null
        m_lReturn = CType(AddInputParameter(v_sName:="task_duration", v_vValue:=v_vTaskDuration, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

        ' calling process key id
        m_lReturn = CType(AddInputParameter(v_sName:="calling_process_key_id", v_vValue:=v_lCallingProcessKeyId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

        ' calling process type id
        m_lReturn = CType(AddInputParameter(v_sName:="calling_process_type_id", v_vValue:=v_lCallingProcessTypeID, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

        ' Execute Action Query
        If m_oDatabase.SQLAction(sSQL:=ACCreateWorkStepDataSQL, sSQLName:=ACCreateWorkStepDataName, bStoredProcedure:=True) <> gPMConstants.PMEReturnCode.PMTrue Then

            result = gPMConstants.PMEReturnCode.PMFalse

            '******************************
            ' Log Error.
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_lTaskInstanceCnt", v_lTaskInstanceCnt)
            oDict.Add("v_lWorkflowStepId", v_lWorkflowStepId)
            oDict.Add("v_vClaimPartyId", v_vClaimPartyId)
            oDict.Add("v_vClaimDebtId", v_vClaimDebtId)
            oDict.Add("v_lCallingProcessKeyId", v_lCallingProcessKeyId)
            oDict.Add("v_lCallingProcessTypeID", v_lCallingProcessTypeID)
            gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=New Exception(Information.Err().Description), oDicParms:=oDict)
            '******************************

        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: CreateWorkStepInstance
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 08-10-2003 : continuation tasks
    ' ***************************************************************** '
    Private Function CreateWorkStepInstance(ByVal v_lWorkflowStepId As Integer, ByVal v_lTaskInstanceCnt As Integer, ByVal v_lCallingProcessKeyId As Integer, ByVal v_lCallingProcessTypeID As Integer) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "CreateWorkStepInstance"



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear Down Database Parameters
        m_oDatabase.Parameters.Clear()

        ' Add Required Stored Procedure Parameters
        m_lReturn = CType(AddInputParameter(v_sName:="pmwrk_workflow_step_id", v_vValue:=v_lWorkflowStepId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

        m_lReturn = CType(AddInputParameter(v_sName:="pmwrk_task_instance_cnt", v_vValue:=v_lTaskInstanceCnt, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

        m_lReturn = CType(AddInputParameter(v_sName:="calling_process_key_id", v_vValue:=v_lCallingProcessKeyId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

        m_lReturn = CType(AddInputParameter(v_sName:="calling_process_type_id", v_vValue:=v_lCallingProcessTypeID, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

        ' Execute Action Query
        If m_oDatabase.SQLAction(sSQL:=ACCreateWorkStepInstanceSQL, sSQLName:=ACCreateWorkStepInstanceName, bStoredProcedure:=True) <> gPMConstants.PMEReturnCode.PMTrue Then

            result = gPMConstants.PMEReturnCode.PMFalse

            '******************************
            ' Log Error.
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_lWorkflowStepId", v_lWorkflowStepId)
            oDict.Add("v_lTaskInstanceCnt", v_lTaskInstanceCnt)
            oDict.Add("v_lCallingProcessKeyId", v_lCallingProcessKeyId)
            oDict.Add("v_lCallingProcessTypeID", v_lCallingProcessTypeID)
            gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=New Exception(Information.Err().Description), oDicParms:=oDict)
            '******************************

        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: CreateWorkStepGroupUser
    '
    ' Parameters: UserId - can be null
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 08-10-2003 : continuation tasks
    ' ***************************************************************** '
    Private Function CreateWorkStepGroupUser(ByVal v_lTaskInstanceCnt As Integer, ByVal v_lWorkflowStepId As Integer, ByVal v_lUserGroupId As Integer, ByVal v_vUserId As Object, ByVal v_lCallingProcessKeyId As Integer, ByVal v_lCallingProcessTypeID As Integer, ByVal v_lBranchId As Integer) As Integer


        Dim result As Integer = 0
        Const sFunctionName As String = "CreateWorkStepGroupUser"



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear Down Database Parameters
        m_oDatabase.Parameters.Clear()

        ' Add Required Stored Procedure Parameters

        ' task instance cnt
        m_lReturn = CType(AddInputParameter(v_sName:="pmwrk_task_instance_cnt", v_vValue:=v_lTaskInstanceCnt, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

        ' workflow step id
        m_lReturn = CType(AddInputParameter(v_sName:="pmwrk_workflow_step_id", v_vValue:=v_lWorkflowStepId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

        ' user group id
        m_lReturn = CType(AddInputParameter(v_sName:="pmuser_group_id", v_vValue:=v_lUserGroupId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

        ' user id - can be null
        m_lReturn = CType(AddInputParameter(v_sName:="user_id", v_vValue:=v_vUserId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

        ' calling process key id
        m_lReturn = CType(AddInputParameter(v_sName:="calling_process_key_id", v_vValue:=v_lCallingProcessKeyId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

        ' calling process type id
        m_lReturn = CType(AddInputParameter(v_sName:="calling_process_type_id", v_vValue:=v_lCallingProcessTypeID, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

        ' branch id
        m_lReturn = CType(AddInputParameter(v_sName:="source_id", v_vValue:=v_lBranchId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

        ' Execute Action Query
        If m_oDatabase.SQLAction(sSQL:=ACCreateWorkStepGroupUserSQL, sSQLName:=ACCreateWorkStepGroupUserName, bStoredProcedure:=True) <> gPMConstants.PMEReturnCode.PMTrue Then

            result = gPMConstants.PMEReturnCode.PMFalse

            '******************************
            ' Log Error.
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_lTaskInstanceCnt", v_lTaskInstanceCnt)
            oDict.Add("v_lWorkflowStepId", v_lWorkflowStepId)
            oDict.Add("v_lUserGroupId", v_lUserGroupId)
            oDict.Add("v_vUserId", v_vUserId)
            oDict.Add("v_lCallingProcessKeyId", v_lCallingProcessKeyId)
            oDict.Add("v_lCallingProcessTypeID", v_lCallingProcessTypeID)
            oDict.Add("v_lBranchId", v_lBranchId)
            gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=New Exception(Information.Err().Description), oDicParms:=oDict)
            '******************************

        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: ProcessStepData
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 08-10-2003 : continuation tasks
    ' ***************************************************************** '
    Private Function ProcessStepData(ByVal v_lCallingProcessKeyId As Integer, ByVal v_lCallingProcessTypeID As Integer, ByVal v_lPMWrkTaskInstanceCnt As Integer, ByVal v_lStartStepCnt As Integer, ByVal v_vStepData(,) As Object) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "ProcessStepData"

        Dim llBound, lUbound As Integer

        Dim lStepId As Integer
        Dim vClaimPartyId As String = ""
        Dim vClaimDebtId As String = ""
        Dim vTaskDescription As String = ""
        Dim vTaskDuration As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        If Information.IsArray(v_vStepData) Then

            llBound = v_vStepData.GetLowerBound(1)
            lUbound = v_vStepData.GetUpperBound(1)

            For lItem As Integer = llBound To lUbound

                ' mandatory always needs to be set

                lStepId = CInt(v_vStepData(ACStepDataStepItemId, lItem))

                ' optional can be null

                ' claim party id

                If CStr(v_vStepData(ACStepDataClaimPartyId, lItem)) = "" Then

                    vClaimPartyId = Nothing
                Else

                    vClaimPartyId = CStr(v_vStepData(ACStepDataClaimPartyId, lItem))
                End If

                ' claim debt id

                If CStr(v_vStepData(ACStepDataClaimDebtId, lItem)) = "" Then

                    vClaimDebtId = Nothing
                Else

                    vClaimDebtId = CStr(v_vStepData(ACStepDataClaimDebtId, lItem))
                End If

                ' task description

                If CStr(v_vStepData(ACStepDataTaskDescription, lItem)) = "" Then

                    vTaskDescription = Nothing
                Else

                    vTaskDescription = CStr(v_vStepData(ACStepDataTaskDescription, lItem))
                End If

                ' task duration

                If CStr(v_vStepData(ACStepDataTaskDuration, lItem)) = "" Then

                    vTaskDuration = Nothing
                Else

                    vTaskDuration = CStr(v_vStepData(ACStepDataTaskDuration, lItem))
                End If

                'Create Appropriate record in work_pmwrk_workflow_stepdata
                If CreateWorkStepData(v_lTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt, v_lWorkflowStepId:=lStepId, v_vClaimPartyId:=vClaimPartyId, v_vClaimDebtId:=vClaimDebtId, v_vTaskDescription:=vTaskDescription, v_vTaskDuration:=vTaskDuration, v_lCallingProcessKeyId:=v_lCallingProcessKeyId, v_lCallingProcessTypeID:=v_lCallingProcessTypeID) <> gPMConstants.PMEReturnCode.PMTrue Then

                    result = gPMConstants.PMEReturnCode.PMFalse

                    Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                    oDict.Add("v_lCallingProcessKeyId", v_lCallingProcessKeyId)
                    oDict.Add("v_lCallingProcessTypeID", v_lCallingProcessTypeID)
                    oDict.Add("v_lPMWrkTaskInstanceCnt", v_lPMWrkTaskInstanceCnt)
                    oDict.Add("v_lStartStepCnt", v_lStartStepCnt)
                    gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to create entry in work_pwmrk_workflow_stepdata for stepId" & CStr(v_lStartStepCnt), vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, oDicParms:=oDict)

                    Exit For
                End If

            Next lItem

        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name:
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 08-10-2003 : continuation tasks
    ' ***************************************************************** '
    '
    Private Function ProcessStepGroupUser(ByVal v_lCallingProcessKeyId As Integer, ByVal v_lCallingProcessTypeID As Integer, ByVal v_lPMWrkTaskInstanceCnt As Integer, ByVal v_lStartStepCnt As Integer, Optional ByVal v_vStepGroupUsers(,) As Object = Nothing) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "ProcessStepGroupUser"

        Dim llBound, lUbound As Integer

        Dim lStepId, lUserGroupId, lBranchId As Integer
        Dim vUserId As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        If Information.IsArray(v_vStepGroupUsers) Then

            llBound = v_vStepGroupUsers.GetLowerBound(1)
            lUbound = v_vStepGroupUsers.GetUpperBound(1)

            For lItem As Integer = llBound To lUbound

                ' mandatory always needs to be set

                lStepId = CInt(v_vStepGroupUsers(ACStepGroupUserStepItemId, lItem))

                lUserGroupId = CInt(v_vStepGroupUsers(ACStepGroupUserUserGroupId, lItem))

                lBranchId = CInt(v_vStepGroupUsers(ACStepGroupUserBranchID, lItem))

                ' optional

                If CStr(v_vStepGroupUsers(ACStepGroupUserUserId, lItem)) = "" Then

                    vUserId = Nothing
                Else

                    vUserId = CInt(v_vStepGroupUsers(ACStepGroupUserUserId, lItem))
                End If

                If CreateWorkStepGroupUser(v_lTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt, v_lWorkflowStepId:=lStepId, v_lUserGroupId:=lUserGroupId, v_vUserId:=vUserId, v_lCallingProcessKeyId:=v_lCallingProcessKeyId, v_lCallingProcessTypeID:=v_lCallingProcessTypeID, v_lBranchId:=lBranchId) <> gPMConstants.PMEReturnCode.PMTrue Then


                    result = gPMConstants.PMEReturnCode.PMError

                    Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                    oDict.Add("v_lCallingProcessKeyId", v_lCallingProcessKeyId)
                    oDict.Add("v_lCallingProcessTypeID", v_lCallingProcessTypeID)
                    oDict.Add("v_lPMWrkTaskInstanceCnt", v_lPMWrkTaskInstanceCnt)
                    oDict.Add("v_lStartStepCnt", v_lStartStepCnt)
                    gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to create stepusergroup " & " entry for Step Id:" & CStr(lStepId), vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, oDicParms:=oDict)

                    Exit For
                End If

            Next lItem

        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: DeleteWorkWorkflowEntries
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 08-10-2003 : continuation tasks
    ' ***************************************************************** '
    Public Function DeleteWorkWorkflowEntries(ByVal v_lCallingProcessKeyId As Integer, ByVal v_lCallingProcessTypeID As Integer) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "DeleteWorkWorkflowEntries"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters

            ' calling process key id
            m_lReturn = CType(AddInputParameter(v_sName:="calling_process_key_id", v_vValue:=v_lCallingProcessKeyId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' calling process type id
            m_lReturn = CType(AddInputParameter(v_sName:="calling_process_type_id", v_vValue:=v_lCallingProcessTypeID, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' Execute Action Query
            If m_oDatabase.SQLAction(sSQL:=ACDeleteWorkWorkflowEntriesSQL, sSQLName:=ACDeleteWorkWorkflowEntriesName, bStoredProcedure:=True) <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse

                '******************************
                ' Log Error.
                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("v_lCallingProcessKeyId", v_lCallingProcessKeyId)
                oDict.Add("v_lCallingProcessTypeID", v_lCallingProcessTypeID)
                gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to delete work workflow entries for " & " calling_process_key_id " & CStr(v_lCallingProcessKeyId) & " calling process type id " & CStr(v_lCallingProcessTypeID), vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=New Exception(Information.Err().Description), oDicParms:=oDict)
                '******************************

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error.
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_lCallingProcessKeyId", v_lCallingProcessKeyId)
            oDict.Add("v_lCallingProcessTypeID", v_lCallingProcessTypeID)
            gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep, oDicParms:=oDict)
            '******************************

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CopyWorkToLive
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 08-10-2003 : Continuation Tasks
    ' ***************************************************************** '
    Public Function CopyWorkToLive(ByVal v_lCallingProcessKeyId As Integer, ByVal v_lCallingProcessTypeID As Integer) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "CopyWorkToLive"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters

            ' calling process key id
            m_lReturn = CType(AddInputParameter(v_sName:="calling_process_key_id", v_vValue:=v_lCallingProcessKeyId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' calling process type id
            m_lReturn = CType(AddInputParameter(v_sName:="calling_process_type_id", v_vValue:=v_lCallingProcessTypeID, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' Execute Action Query
            If m_oDatabase.SQLAction(sSQL:=ACCopyWorkToLiveSQL, sSQLName:=ACCopyWorkToLiveName, bStoredProcedure:=True) <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse

                '******************************
                ' Log Error.
                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("v_lCallingProcessKeyId", v_lCallingProcessKeyId)
                oDict.Add("v_lCallingProcessTypeID", v_lCallingProcessTypeID)
                gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=New Exception(Information.Err().Description), oDicParms:=oDict)
                '******************************

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error.
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_lCallingProcessKeyId", v_lCallingProcessKeyId)
            oDict.Add("v_lCallingProcessTypeID", v_lCallingProcessTypeID)
            gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep, oDicParms:=oDict)
            '******************************

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ProcessStep
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 09-10-2003 : continuation tasks
    ' ***************************************************************** '
    Public Function ProcessStep(ByVal v_lStepInstanceCnt As Integer, ByVal v_lTaskInstanceCnt As Integer, ByVal v_lStepId As Integer) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "ProcessStep"

        Dim vStepDetails As Object
        Dim lTaskDuration, lNextInTimeStepId As Integer
        Dim vDefaultCompletionTaskOutcomeId, sDocTemplateCode As String
        Dim lInsuranceFileCnt, lInsuranceFolderCnt, lPartyCnt, lClaimCnt, lWorkflowPackageId As Integer
        Dim sComponentName As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' get the workflow step details
            If GetStepDetails(v_lStepInstanceCnt:=v_lStepInstanceCnt, r_vResults:=vStepDetails) = gPMConstants.PMEReturnCode.PMTrue Then

                ' get the package id

                lWorkflowPackageId = CInt(vStepDetails(ACStepWorkflowId, 0))

                ' get the document template id

                If CStr(vStepDetails(ACStepDocTemplateCode, 0)).Trim() <> "" Then

                    ' get associated doc details

                    sDocTemplateCode = CStr(vStepDetails(ACStepDocTemplateCode, 0))

                    lInsuranceFileCnt = CInt(vStepDetails(ACStepEventLogInsuranceFileCnt, 0))

                    lInsuranceFolderCnt = CInt(vStepDetails(ACStepEventLogInsuranceFolderCnt, 0))

                    lPartyCnt = CInt(vStepDetails(ACStepEventLogPartyCnt, 0))

                    lClaimCnt = CInt(vStepDetails(ACStepEventLogClaimCnt, 0))

                    ' produce associated document
                    If ProduceAssociatedDocument(v_sDocTemplateCode:=sDocTemplateCode, v_lInsuranceFileCnt:=lInsuranceFileCnt, v_lInsuranceFolderCnt:=lInsuranceFolderCnt, v_lPartyCnt:=lPartyCnt, v_lClaimCnt:=lClaimCnt) <> gPMConstants.PMEReturnCode.PMTrue Then

                        result = gPMConstants.PMEReturnCode.PMFalse

                        Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                        oDict.Add("v_lStepInstanceCnt", v_lStepInstanceCnt)
                        oDict.Add("v_lTaskInstanceCnt", v_lTaskInstanceCnt)
                        oDict.Add("v_lStepId", v_lStepId)
                        gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to process associated document for task instance:" & CStr(v_lTaskInstanceCnt), vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, oDicParms:=oDict)

                    End If

                End If

                ' use override value if it is set

                If CStr(vStepDetails(ACStepSDTaskDuration, 0)) <> "" Then

                    lTaskDuration = CInt(vStepDetails(ACStepSDTaskDuration, 0))
                Else
                    ' else use default

                    lTaskDuration = CInt(vStepDetails(ACStepStepDaysDuration, 0))
                End If

                ' determine if this task is required to be executed immediately
                If lTaskDuration = ACProcessTaskImmediately Then

                    ' if this task is executable

                    If CDbl(vStepDetails(ACStepExecutableTask, 0)) = ACExecuteTask Then

                        ' get the specified task instance keys
                        If GetTaskInstKeys(v_lTaskInstanceCnt:=v_lTaskInstanceCnt) = gPMConstants.PMEReturnCode.PMTrue Then

                            ' need to get the name of the component that needs to be executed...

                            sComponentName = CStr(vStepDetails(ACStepExecutableComponent, 0))

                            ' execute the task
                            If ExecuteGenericTask(v_sObjectName:=sComponentName, v_vTaskKeys:=m_vTaskKeys) = gPMConstants.PMEReturnCode.PMTrue Then
                                ' get default completion task outcome for task

                                If CStr(vStepDetails(ACStepTaskDefaultCompletionTaskOutcome, 0)) = "" Then

                                    vDefaultCompletionTaskOutcomeId = Nothing
                                Else

                                    vDefaultCompletionTaskOutcomeId = CStr(vStepDetails(ACStepTaskDefaultCompletionTaskOutcome, 0))
                                End If


                                ' update task to indicate completed....
                                If CompleteTask(v_lTaskInstanceCnt:=v_lTaskInstanceCnt, v_vDefaultCompletionTaskOutcomeId:=vDefaultCompletionTaskOutcomeId) = gPMConstants.PMEReturnCode.PMTrue Then

                                    ' get the next step to be processed now that this one is complete

                                    If CStr(vStepDetails(ACStepCompleteNextWorkflowStepId, 0)) <> "" Then

                                        lNextInTimeStepId = CInt(vStepDetails(ACStepCompleteNextWorkflowStepId, 0))
                                    End If

                                    ' process the next step in the package
                                    If ProcessNextStep(v_lStepInstanceCnt:=v_lStepInstanceCnt, v_lTaskInstanceCnt:=v_lTaskInstanceCnt, v_lWorkflowPackageId:=lWorkflowPackageId, v_lNextStepId:=lNextInTimeStepId) <> gPMConstants.PMEReturnCode.PMTrue Then

                                        result = gPMConstants.PMEReturnCode.PMFalse

                                        Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                                        oDict.Add("v_lStepInstanceCnt", v_lStepInstanceCnt)
                                        oDict.Add("v_lTaskInstanceCnt", v_lTaskInstanceCnt)
                                        oDict.Add("v_lStepId", v_lStepId)
                                        gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to process the next step in the package:" & CStr(lNextInTimeStepId), vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, oDicParms:=oDict)
                                    End If

                                Else

                                    result = gPMConstants.PMEReturnCode.PMFalse

                                    Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                                    oDict.Add("v_lStepInstanceCnt", v_lStepInstanceCnt)
                                    oDict.Add("v_lTaskInstanceCnt", v_lTaskInstanceCnt)
                                    oDict.Add("v_lStepId", v_lStepId)
                                    gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to update task to completed:" & CStr(v_lTaskInstanceCnt), vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, oDicParms:=oDict)

                                End If

                            Else

                                result = gPMConstants.PMEReturnCode.PMFalse

                                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                                oDict.Add("v_lStepInstanceCnt", v_lStepInstanceCnt)
                                oDict.Add("v_lTaskInstanceCnt", v_lTaskInstanceCnt)
                                oDict.Add("v_lStepId", v_lStepId)
                                gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to execute task:" & CStr(v_lStepId), vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, oDicParms:=oDict)

                            End If

                        Else
                            result = gPMConstants.PMEReturnCode.PMFalse

                            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                            oDict.Add("v_lStepInstanceCnt", v_lStepInstanceCnt)
                            oDict.Add("v_lTaskInstanceCnt", v_lTaskInstanceCnt)
                            oDict.Add("v_lStepId", v_lStepId)
                            gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to get task inst keys for specified task:" & CStr(v_lTaskInstanceCnt), vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, oDicParms:=oDict)

                        End If

                    End If

                End If

            Else

                result = gPMConstants.PMEReturnCode.PMFalse

                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("v_lStepInstanceCnt", v_lStepInstanceCnt)
                oDict.Add("v_lTaskInstanceCnt", v_lTaskInstanceCnt)
                oDict.Add("v_lStepId", v_lStepId)
                gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to get details for specified step:" & CStr(v_lStepId), vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, oDicParms:=oDict)

            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error.
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_lStepInstanceCnt", v_lStepInstanceCnt)
            oDict.Add("v_lTaskInstanceCnt", v_lTaskInstanceCnt)
            oDict.Add("v_lStepId", v_lStepId)
            gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep, oDicParms:=oDict)
            '******************************

            Return result


            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetStepDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 09-10-2003 : Continuation Tasks
    ' ***************************************************************** '
    Private Function GetStepDetails(ByVal v_lStepInstanceCnt As Integer, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "GetStepDetails"



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear Down Database Parameters
        m_oDatabase.Parameters.Clear()

        ' Add Required Stored Procedure Parameters
        m_lReturn = CType(AddInputParameter(v_sName:="pmwrk_workflow_step_instance_cnt", v_vValue:=v_lStepInstanceCnt, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

        ' Execute selection Query
        If m_oDatabase.SQLSelect(sSQL:=ACGetStepDetailsSQL, sSQLName:=ACGetStepDetailsName, bStoredProcedure:=True, vResultArray:=r_vResults) <> gPMConstants.PMEReturnCode.PMTrue Then

            result = gPMConstants.PMEReturnCode.PMFalse

            '******************************
            ' Log Error.
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_lStepInstanceCnt", v_lStepInstanceCnt)
            gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to retrieve step details", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=New Exception(Information.Err().Description), oDicParms:=oDict)
            '******************************

        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: ExecuteGenericTask
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 09-10-2003 : Continuation Tasks
    ' ***************************************************************** '
    Private Function ExecuteGenericTask(ByVal v_sObjectName As String, ByVal v_vTaskKeys As Object) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "ExecuteGenericTask"

        Dim oObject As Object

        result = gPMConstants.PMEReturnCode.PMTrue

       
        ' confirm we have a valid component object name
        ' before trying to execute task
        If v_sObjectName <> "." And v_sObjectName <> "" Then

            If gPMComponentServices.CreateBusinessObject(r_oObject:=oObject, v_sClassName:=v_sObjectName, v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase) = gPMConstants.PMEReturnCode.PMTrue Then




                If oObject.SetKeys(vKeyArray:=v_vTaskKeys) = gPMConstants.PMEReturnCode.PMTrue Then


                    If oObject.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate) = gPMConstants.PMEReturnCode.PMTrue Then


                        If oObject.Start() <> gPMConstants.PMEReturnCode.PMTrue Then

                            result = gPMConstants.PMEReturnCode.PMFalse

                            '******************************
                            ' Log Error.
                            gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to start :" & v_sObjectName, vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=New Exception(Information.Err().Description))
                            '******************************

                        End If

                    Else
                        result = gPMConstants.PMEReturnCode.PMFalse

                        '******************************
                        ' Log Error.
                        gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to setprocessmodes for :" & v_sObjectName, vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=New Exception(Information.Err().Description))
                        '******************************

                    End If
                Else
                    result = gPMConstants.PMEReturnCode.PMFalse

                    '******************************
                    ' Log Error.
                    gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to setkeys for :" & v_sObjectName, vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=New Exception(Information.Err().Description))
                    '******************************

                End If

            Else

                result = gPMConstants.PMEReturnCode.PMFalse

                '******************************
                ' Log Error.
                gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to createbusinessobject:" & v_sObjectName, vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=New Exception(Information.Err().Description))
                '******************************

            End If

        End If

        oObject = Nothing

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetTaskInstKeys
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 09-10-2003 : Continuation Tasks
    ' ***************************************************************** '
    Private Function GetTaskInstKeys(ByVal v_lTaskInstanceCnt As Integer) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "GetTaskInstKeys"



        result = gPMConstants.PMEReturnCode.PMTrue

        ' the keys are going to be the same for each step in the package
        ' so only load them once...
        If Not Information.IsArray(m_vTaskKeys) Then

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters

            ' task instance cnt
            m_lReturn = CType(AddInputParameter(v_sName:="pmwrk_task_instance_cnt", v_vValue:=v_lTaskInstanceCnt, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' Execute selection Query
            If m_oDatabase.SQLSelect(sSQL:=ACGetTaskInstKeysSQL, sSQLName:=ACGetTaskInstKeysName, bStoredProcedure:=True, vResultArray:=m_vTaskKeys) <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse

                '******************************
                ' Log Error.
                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("v_lTaskInstanceCnt", v_lTaskInstanceCnt)
                gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to get the keys for the specified pmwrk_task_instance_cnt", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=New Exception(Information.Err().Description), oDicParms:=oDict)
                '******************************

            End If

        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: CopyTaskInstKeys
    '
    ' Parameters: n/a
    '
    ' Description: Copies the task instance keys from specified task
    '               to specified task
    '
    ' History:
    '           Created : MEvans : 10-10-2003 : Continuation Tasks
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (CopyTaskInstKeys) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function CopyTaskInstKeys(ByVal v_lCopyFromTaskInstanceCnt As Integer, ByVal v_lCopyToTaskInstanceCnt As Integer) As Integer
    '
    'Dim result As Integer = 0
    'Const sFunctionName As String = "CopyTaskInstKeys"
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Clear Down Database Parameters
    'm_oDatabase.Parameters.Clear()
    '
    ' Add Required Stored Procedure Parameters
    '
    ' Copy from
    'm_lReturn = CType(AddInputParameter(v_sName:="copyfrom_task_instance_cnt", v_vValue:=v_lCopyFromTaskInstanceCnt, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
    '
    ' copy to
    'm_lReturn = CType(AddInputParameter(v_sName:="copyto_task_instance_cnt", v_vValue:=v_lCopyToTaskInstanceCnt, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
    '
    ' Execute Action Query
    'If m_oDatabase.SQLAction(sSQL:=ACCopyTaskInstKeysSQL, sSQLName:=ACCopyTaskInstKeysName, bStoredProcedure:=True) <> gPMConstants.PMEReturnCode.PMTrue Then
    '
    'result = gPMConstants.PMEReturnCode.PMFalse
    '
    '******************************
    ' Log Error.
    'gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)
    '******************************
    '
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    '******************************
    ' Log Error.
    'gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
    '******************************
    '
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: UpdateStepInstance
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 10-10-2003 : continuation tasks
    ' ***************************************************************** '
    Private Function UpdateStepInstance(ByVal v_lStepId As Integer, ByVal v_lTaskInstanceCnt As Integer, ByVal v_lStepInstanceCnt As Integer) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "UpdateStepInstance"



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear Down Database Parameters
        m_oDatabase.Parameters.Clear()

        ' Add Required Stored Procedure Parameters
        m_lReturn = CType(AddInputParameter(v_sName:="pmwrk_workflow_step_id", v_vValue:=v_lStepId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

        m_lReturn = CType(AddInputParameter(v_sName:="pmwrk_task_instance_cnt", v_vValue:=v_lTaskInstanceCnt, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

        m_lReturn = CType(AddInputParameter(v_sName:="pmwrk_workflow_step_instance_cnt", v_vValue:=v_lStepInstanceCnt, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

        ' Execute Action Query
        If m_oDatabase.SQLAction(sSQL:=ACUpdateStepInstanceSQL, sSQLName:=ACUpdateStepInstanceName, bStoredProcedure:=True) <> gPMConstants.PMEReturnCode.PMTrue Then

            result = gPMConstants.PMEReturnCode.PMFalse

            '******************************
            ' Log Error.
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_lStepId", v_lStepId)
            oDict.Add("v_lTaskInstanceCnt", v_lTaskInstanceCnt)
            oDict.Add("v_lStepInstanceCnt", v_lStepInstanceCnt)
            gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=New Exception(Information.Err().Description), oDicParms:=oDict)
            '******************************

        End If

        Return result

    End Function

    
    ' ***************************************************************** '
    ' Name: CompleteTask
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 10-10-2003 : Continuation Task
    ' ***************************************************************** '
    Private Function CompleteTask(ByVal v_lTaskInstanceCnt As Integer, ByVal v_vDefaultCompletionTaskOutcomeId As Object) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "CompleteTask"

        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear Down Database Parameters
        m_oDatabase.Parameters.Clear()

        ' Add Required Stored Procedure Parameters
        m_lReturn = CType(AddInputParameter(v_sName:="pmwrk_task_instance_cnt", v_vValue:=v_lTaskInstanceCnt, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

        m_lReturn = CType(AddInputParameter(v_sName:="task_outcome_id", v_vValue:=v_vDefaultCompletionTaskOutcomeId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

        m_lReturn = CType(AddInputParameter(v_sName:="task_status", v_vValue:=ACTaskStatusCompleted, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

        ' Execute Action Query
        If m_oDatabase.SQLAction(sSQL:=ACCompleteTaskSQL, sSQLName:=ACCompleteTaskName, bStoredProcedure:=True) <> gPMConstants.PMEReturnCode.PMTrue Then

            result = gPMConstants.PMEReturnCode.PMFalse

            '******************************
            ' Log Error.
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_lTaskInstanceCnt", v_lTaskInstanceCnt)
            oDict.Add("v_vDefaultCompletionTaskOutcomeId", v_vDefaultCompletionTaskOutcomeId)
            gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=New Exception(Information.Err().Description), oDicParms:=oDict)
            '******************************

        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: CreateEventTask
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 10-10-2003 : Continuation Tasks
    ' ***************************************************************** '
    Function CreateEventTask(ByRef r_lTaskInstanceCnt As Integer, ByRef r_lEventCnt As Integer, ByVal v_lTaskGroupId As Integer, ByVal v_iTaskIsVisible As Integer, ByVal v_lTaskId As Integer, ByVal v_lTaskActionTypeId As Integer, ByVal v_sTaskCustomer As String, ByVal v_dtTaskDueDate As Date, ByVal v_lTaskPMUserGroupId As Integer, ByVal v_sTaskDescription As String, ByVal v_iTaskStatus As Integer, ByVal v_iTaskIsUrgent As Integer, ByVal v_sTaskWorkflowInformation As String, ByVal v_iTaskUserId As Integer, ByVal v_vEventPartyCnt As Object, ByVal v_vEventInsuranceFolderCnt As Object, ByVal v_vEventInsuranceFileCnt As Object, ByVal v_vEventClaimCnt As Object, ByVal v_vEventTypeCode As Object, ByVal v_vEventDescription As Object, ByVal v_vTaskInstanceKeyArray As Object, ByVal v_vEventClaimPartyId As Integer, ByVal v_vEventClaimDebtId As Integer, ByVal v_vEventLogSubjectId As Object, ByVal v_vEventClaimPerilId As Object, ByVal v_iTaskSourceId As Integer) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "CreateEventTask"

        Dim oEventTask As bPMEventTask.Business
        Try


            result = gPMConstants.PMEReturnCode.PMTrue
            oEventTask = New bPMEventTask.Business
            If oEventTask.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase) = gPMConstants.PMEReturnCode.PMTrue Then

                ' Create Event Task

                If oEventTask.CreateEventTask(r_lTaskInstanceCnt:=r_lTaskInstanceCnt, r_vEventCnt:=r_lEventCnt, v_lTaskGroupID:=v_lTaskGroupId, v_iTaskIsVisible:=v_iTaskIsVisible, v_lTaskID:=v_lTaskId, v_lTaskActionTypeID:=v_lTaskActionTypeId, v_sTaskCustomer:=v_sTaskCustomer, v_dtTaskDueDate:=v_dtTaskDueDate, v_lTaskPMUserGroupID:=v_lTaskPMUserGroupId, v_sTaskDescription:=v_sTaskDescription, v_iTaskStatus:=v_iTaskStatus, v_iTaskIsUrgent:=v_iTaskIsUrgent, v_sTaskWorkflowInformation:=v_sTaskWorkflowInformation, v_iTaskUserID:=v_iTaskUserId, v_vEventPartyCnt:=v_vEventPartyCnt, v_vEventInsuranceFolderCnt:=v_vEventInsuranceFolderCnt, v_vEventInsuranceFileCnt:=v_vEventInsuranceFileCnt, v_vEventClaimCnt:=v_vEventClaimCnt, v_vEventTypeCode:=v_vEventTypeCode, v_vEventDescription:=v_vEventDescription, v_vTaskInstanceKeyArray:=v_vTaskInstanceKeyArray, v_lEventClaimPartyId:=v_vEventClaimPartyId, v_lEventClaimDebtID:=v_vEventClaimDebtId, v_vEventLogSubjectId:=v_vEventLogSubjectId, v_vEventClaimPerilId:=v_vEventClaimPerilId, v_iTaskSourceId:=v_iTaskSourceId) <> gPMConstants.PMEReturnCode.PMTrue Then

                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error.
                    Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                    oDict.Add("r_lTaskInstanceCnt", r_lTaskInstanceCnt)
                    oDict.Add("r_lEventCnt", r_lEventCnt)
                    oDict.Add("v_lTaskGroupId", v_lTaskGroupId)
                    oDict.Add("v_lTaskId", v_lTaskId)
                    oDict.Add("v_lTaskActionTypeId", v_lTaskActionTypeId)
                    oDict.Add("v_dtTaskDueDate", v_dtTaskDueDate)
                    oDict.Add("v_lTaskPMUserGroupId", v_lTaskPMUserGroupId)
                    oDict.Add("v_iTaskUserId", v_iTaskUserId)
                    oDict.Add("v_vEventPartyCnt", v_vEventPartyCnt)
                    oDict.Add("v_vEventInsuranceFolderCnt", v_vEventInsuranceFolderCnt)
                    oDict.Add("v_vEventInsuranceFileCnt", v_vEventInsuranceFileCnt)
                    oDict.Add("v_vEventClaimCnt", v_vEventClaimCnt)
                    oDict.Add("v_vEventTypeCode", v_vEventTypeCode)
                    oDict.Add("v_vEventClaimPartyId", v_vEventClaimPartyId)
                    oDict.Add("v_vEventClaimDebtId", v_vEventClaimDebtId)
                    oDict.Add("v_vEventLogSubjectId", v_vEventLogSubjectId)
                    oDict.Add("v_vEventClaimPerilId", v_vEventClaimPerilId)
                    oDict.Add("v_iTaskSourceId", v_iTaskSourceId)
                    gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to create event task", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, oDicParms:=oDict)

                End If

            Else

                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("r_lTaskInstanceCnt", r_lTaskInstanceCnt)
                oDict.Add("r_lEventCnt", r_lEventCnt)
                oDict.Add("v_lTaskGroupId", v_lTaskGroupId)
                oDict.Add("v_lTaskId", v_lTaskId)
                oDict.Add("v_lTaskActionTypeId", v_lTaskActionTypeId)
                oDict.Add("v_dtTaskDueDate", v_dtTaskDueDate)
                oDict.Add("v_lTaskPMUserGroupId", v_lTaskPMUserGroupId)
                oDict.Add("v_iTaskUserId", v_iTaskUserId)
                oDict.Add("v_vEventPartyCnt", v_vEventPartyCnt)
                oDict.Add("v_vEventInsuranceFolderCnt", v_vEventInsuranceFolderCnt)
                oDict.Add("v_vEventInsuranceFileCnt", v_vEventInsuranceFileCnt)
                oDict.Add("v_vEventClaimCnt", v_vEventClaimCnt)
                oDict.Add("v_vEventTypeCode", v_vEventTypeCode)
                oDict.Add("v_vEventClaimPartyId", v_vEventClaimPartyId)
                oDict.Add("v_vEventClaimDebtId", v_vEventClaimDebtId)
                oDict.Add("v_vEventLogSubjectId", v_vEventLogSubjectId)
                oDict.Add("v_vEventClaimPerilId", v_vEventClaimPerilId)
                oDict.Add("v_iTaskSourceId", v_iTaskSourceId)
                gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to create component bPMEventTask.Business", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, oDicParms:=oDict)

            End If

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error.
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("r_lTaskInstanceCnt", r_lTaskInstanceCnt)
            oDict.Add("r_lEventCnt", r_lEventCnt)
            oDict.Add("v_lTaskGroupId", v_lTaskGroupId)
            oDict.Add("v_lTaskId", v_lTaskId)
            oDict.Add("v_lTaskActionTypeId", v_lTaskActionTypeId)
            oDict.Add("v_dtTaskDueDate", v_dtTaskDueDate)
            oDict.Add("v_lTaskPMUserGroupId", v_lTaskPMUserGroupId)
            oDict.Add("v_iTaskUserId", v_iTaskUserId)
            oDict.Add("v_vEventPartyCnt", v_vEventPartyCnt)
            oDict.Add("v_vEventInsuranceFolderCnt", v_vEventInsuranceFolderCnt)
            oDict.Add("v_vEventInsuranceFileCnt", v_vEventInsuranceFileCnt)
            oDict.Add("v_vEventClaimCnt", v_vEventClaimCnt)
            oDict.Add("v_vEventTypeCode", v_vEventTypeCode)
            oDict.Add("v_vEventClaimPartyId", v_vEventClaimPartyId)
            oDict.Add("v_vEventClaimDebtId", v_vEventClaimDebtId)
            oDict.Add("v_vEventLogSubjectId", v_vEventLogSubjectId)
            oDict.Add("v_vEventClaimPerilId", v_vEventClaimPerilId)
            oDict.Add("v_iTaskSourceId", v_iTaskSourceId)
            gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep, oDicParms:=oDict)
            '******************************


        Finally
            ' destroy object reference...
        End Try



        Return result

    End Function

    ' ***************************************************************** '
    ' Name: CreateNewEventTask
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 13-10-2003 : Continuation Tasks
    ' ***************************************************************** '
    Private Function CreateNewEventTask(ByRef r_lTaskInstanceCnt As Integer, ByRef r_lEventCnt As Integer, ByVal v_vStepDetails(,) As Object) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "CreateNewEventTask"


        Dim lTaskGroupId As Integer
        Dim iTaskIsVisible As Integer
        Dim lTaskId, lTaskActionTypeId As Integer
        Dim sTaskCustomer As String = ""
        Dim dtTaskDueDateAsDate As Date
        Dim lTaskPMUserGroupId As Integer
        Dim sTaskDescription As String = ""
        Dim iTaskStatus, iTaskIsUrgent As Integer
        Dim sTaskWorkflowInformation As String = ""
        Dim iTaskUserId As Integer
        Dim vEventPartyCnt As String = ""
        Dim vEventInsuranceFolderCnt As String = ""
        Dim vEventInsuranceFileCnt As String = ""
        Dim vEventClaimCnt As String = ""
        Dim vEventTypeCode As String = ""
        Dim vEventDescription As String = ""
        Dim vTaskInstanceKeyArray As Object
        Dim vEventClaimPartyId As String = ""
        Dim vEventClaimDebtId As String = ""
        Dim lTaskDuration As Integer
        Dim vEventLogSubjectId As String = ""
        Dim vEventClaimPerilId As String = ""
        Dim iExecutableTask, lBranchId As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        ' mandatory step fields

        lTaskGroupId = CInt(v_vStepDetails(ACStepTaskGroupId, 0))

        lTaskId = CInt(v_vStepDetails(ACStepTaskId, 0))

        vEventTypeCode = CStr(v_vStepDetails(ACStepEventTypeCode, 0))

        vEventLogSubjectId = CStr(v_vStepDetails(ACStepEventLogSubjectId, 0))

        ' optional step fields

        If CStr(v_vStepDetails(ACStepTaskActionTypeId, 0)) = "" Then
            lTaskActionTypeId = 0
        Else

            lTaskActionTypeId = CInt(v_vStepDetails(ACStepTaskActionTypeId, 0))
        End If


        sTaskCustomer = CStr(v_vStepDetails(ACStepTaskCustomer, 0))

        iTaskIsUrgent = CInt(v_vStepDetails(ACStepTaskIsUrgent, 0))

        sTaskWorkflowInformation = CStr(v_vStepDetails(ACStepTaskWorkflow, 0))

        vEventDescription = CStr(v_vStepDetails(ACStepEventDescription, 0))

        ' taken from original tasks instance's event

        vEventPartyCnt = CStr(v_vStepDetails(ACStepEventLogPartyCnt, 0))

        vEventInsuranceFolderCnt = CStr(v_vStepDetails(ACStepEventLogInsuranceFolderCnt, 0))

        vEventInsuranceFileCnt = CStr(v_vStepDetails(ACStepEventLogInsuranceFileCnt, 0))

        vEventClaimCnt = CStr(v_vStepDetails(ACStepEventLogClaimCnt, 0))

        ' default event claim peril id to zero if not set.

        If CStr(v_vStepDetails(ACStepEventClaimPerilId, 0)) = "" Then
            vEventClaimPerilId = CStr(0)
        Else

            vEventClaimPerilId = CStr(v_vStepDetails(ACStepEventClaimPerilId, 0))
        End If

        ' if we execute this later on it will go to complete
        ' but for now default the task status to incomplete
        iTaskStatus = 0

        ' taken from original task instance


        vTaskInstanceKeyArray = m_vTaskKeys

        '****************
        ' Use data overrides if any are present otherwise step data
        ' these override keys are available via StepGroupUser or StepData tables
        '****************

        ' branch id

        If CStr(v_vStepDetails(ACStepSGUBranchId, 0)) <> "" Then

            lBranchId = CInt(v_vStepDetails(ACStepSGUBranchId, 0))
        Else

            lBranchId = CInt(v_vStepDetails(ACStepBranchId, 0))
        End If

        '***********************

        ' task user group id

        If CStr(v_vStepDetails(ACStepSGUPMUserGroupId, 0)) <> "" Then

            lTaskPMUserGroupId = CInt(v_vStepDetails(ACStepSGUPMUserGroupId, 0))
        Else

            lTaskPMUserGroupId = CInt(v_vStepDetails(ACStepUserGroupId, 0))
        End If

        '***********************

        ' task user id

        If CStr(v_vStepDetails(ACStepSGUUserId, 0)) <> "" Then

            iTaskUserId = CInt(v_vStepDetails(ACStepSGUUserId, 0))
        Else

            If CStr(v_vStepDetails(ACStepUserId, 0)) <> "" Then

                iTaskUserId = CInt(v_vStepDetails(ACStepUserId, 0))
            Else
                iTaskUserId = 0
            End If
        End If

        '***********************

        ' Claim Party Id


        If CStr(v_vStepDetails(ACStepSDClaimPartyId, 0)) <> "" Then

            vEventClaimPartyId = CStr(v_vStepDetails(ACStepSDClaimPartyId, 0))
        ElseIf CStr(v_vStepDetails(ACStepEventLogClaimPartyId, 0)) <> "" Then

            vEventClaimPartyId = CStr(v_vStepDetails(ACStepEventLogClaimPartyId, 0))
        Else
            vEventClaimPartyId = CStr(0)
        End If

        '***********************

        ' Claim Debt Id


        If CStr(v_vStepDetails(ACStepSDClaimDebtId, 0)) <> "" Then

            vEventClaimDebtId = CStr(v_vStepDetails(ACStepSDClaimDebtId, 0))
        ElseIf CStr(v_vStepDetails(ACStepEventLogClaimDebtId, 0)) <> "" Then

            vEventClaimDebtId = CStr(v_vStepDetails(ACStepEventLogClaimDebtId, 0))
        Else
            vEventClaimDebtId = CStr(0)
        End If

        '***********************

        ' task description

        If CStr(v_vStepDetails(ACStepSDTaskDescription, 0)) <> "" Then

            sTaskDescription = CStr(v_vStepDetails(ACStepSDTaskDescription, 0))
        Else

            sTaskDescription = CStr(v_vStepDetails(ACStepTaskDescription, 0))
        End If

        '***********************

        ' need to double check what the value is in stepdata array
        ' because this information can be available in two places.
        ' step data array overrides any defaults

        If CStr(v_vStepDetails(ACStepSDTaskDuration, 0)) <> "" Then

            lTaskDuration = CInt(v_vStepDetails(ACStepSDTaskDuration, 0))
        Else

            lTaskDuration = CInt(v_vStepDetails(ACStepStepDaysDuration, 0))
        End If

        ' task due date is determined from the task expected duration
        dtTaskDueDateAsDate = DateTime.Now.AddDays(lTaskDuration)

        '***********************

        ' is the task executable

        iExecutableTask = CInt(v_vStepDetails(ACStepExecutableTask, 0))

        ' only show the task in someones task list if the task isnt supposed to
        ' be executed immediately.
        If lTaskDuration = 0 And iExecutableTask = 1 Then
            iTaskIsVisible = 0
        Else
            iTaskIsVisible = 1
        End If

        '***********************

        If CreateEventTask(r_lTaskInstanceCnt:=r_lTaskInstanceCnt, r_lEventCnt:=r_lEventCnt, v_lTaskGroupId:=lTaskGroupId, v_iTaskIsVisible:=iTaskIsVisible, v_lTaskId:=lTaskId, v_lTaskActionTypeId:=lTaskActionTypeId, v_sTaskCustomer:=sTaskCustomer, v_dtTaskDueDate:=dtTaskDueDateAsDate, v_lTaskPMUserGroupId:=lTaskPMUserGroupId, v_sTaskDescription:=sTaskDescription, v_iTaskStatus:=iTaskStatus, v_iTaskIsUrgent:=iTaskIsUrgent, v_sTaskWorkflowInformation:=sTaskWorkflowInformation, v_iTaskUserId:=iTaskUserId, v_vEventPartyCnt:=vEventPartyCnt, v_vEventInsuranceFolderCnt:=vEventInsuranceFolderCnt, v_vEventInsuranceFileCnt:=vEventInsuranceFileCnt, v_vEventClaimCnt:=vEventClaimCnt, v_vEventTypeCode:=vEventTypeCode, v_vEventDescription:=vEventDescription, v_vTaskInstanceKeyArray:=vTaskInstanceKeyArray, v_vEventClaimPartyId:=CInt(vEventClaimPartyId), v_vEventClaimDebtId:=CInt(vEventClaimDebtId), v_vEventLogSubjectId:=vEventLogSubjectId, v_vEventClaimPerilId:=vEventClaimPerilId, v_iTaskSourceId:=lBranchId) <> gPMConstants.PMEReturnCode.PMTrue Then

            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error.
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("r_lTaskInstanceCnt", r_lTaskInstanceCnt)
            oDict.Add("r_lEventCnt", r_lEventCnt)
            gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to create event task", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, oDicParms:=oDict)

        End If

        Return result

    End Function




    ' ***************************************************************** '
    ' Name: ProduceAssociatedDocument
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 14-10-2003 : Continuation Tasks
    ' ***************************************************************** '
    Private Function ProduceAssociatedDocument(ByVal v_sDocTemplateCode As String, ByVal v_lInsuranceFileCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_lClaimCnt As Integer) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "ProduceAssociatedDocument"

        Dim lTemplateId, lTemplateTypeId As Integer
        Dim sDocDescription As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        If m_oDocManagerWrapper Is Nothing Then

            m_oDocManagerWrapper = New bSIRDocManagerWrapper.Interface_Renamed()

            If m_oDocManagerWrapper.InitialiseBusiness(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName) <> gPMConstants.PMEReturnCode.PMTrue Then

                m_oDocManagerWrapper = Nothing

                result = gPMConstants.PMEReturnCode.PMFalse

                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("v_sDocTemplateCode", v_sDocTemplateCode)
                oDict.Add("v_lInsuranceFileCnt", v_lInsuranceFileCnt)
                oDict.Add("v_lInsuranceFolderCnt", v_lInsuranceFolderCnt)
                oDict.Add("v_lPartyCnt", v_lPartyCnt)
                oDict.Add("v_lClaimCnt", v_lClaimCnt)
                gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to create bSIRDocManagerWrapper.Interface", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, oDicParms:=oDict)

            End If

        End If

        If Not (m_oDocManagerWrapper Is Nothing) Then

            If GetDocTemplateDetails(v_sTemplateCode:=v_sDocTemplateCode, v_vInsuranceFileCnt:=v_lInsuranceFileCnt, r_lTemplateId:=lTemplateId, r_lTemplateTypeId:=lTemplateTypeId, r_sDocDescription:=sDocDescription) Then

                ' Set up document manager wrapper properties
                m_oDocManagerWrapper.DocumentTypeId = lTemplateTypeId
                m_oDocManagerWrapper.DocumentTemplateId = lTemplateId
                m_oDocManagerWrapper.DocName = "" ' sDocDescription
                m_oDocManagerWrapper.ClaimCnt = v_lClaimCnt
                m_oDocManagerWrapper.InsuranceFileCnt = v_lInsuranceFileCnt
                m_oDocManagerWrapper.InsuranceFolderCnt = v_lInsuranceFolderCnt
                m_oDocManagerWrapper.PartyCnt = v_lPartyCnt

                ' if i need to archive as well need to set extras keys claim ref etc
                ' see bSIRDocManagerWrapper.EncryptKeys
                m_oDocManagerWrapper.ArchiveDoc = False


                m_oDocManagerWrapper.Mode = gSIRLibrary.ACSpoolDocMode

                '**********************************
                '**********************************
                '**********************************
                ' will need to set this up if claim specific documents are required
                ' an additional array of document fields which will hold claim debt id
                ' claim peril id , purchase order id

                ' set up additional field stored procedure parameter for
                ' claim debt id, claim peril id, purchase order id
                '            ReDim vFieldParam(0 To 2, 0)
                '            vFieldParam(ACParamName, 0) = ACParamNamePurchaseOrderId
                '            vFieldParam(ACParamValue, 0) = m_lPurchaseOrderId
                '            vFieldParam(ACParamType, 0) = PMLong
                '
                '            Add Field Parameter PurchaseOrderId
                '            m_oDocManagerWrapper.FieldParameters = vFieldParam
                '**********************************
                '**********************************
                '**********************************

                ' Start should create a spooled zipped merged file
                If m_oDocManagerWrapper.Start() <> gPMConstants.PMEReturnCode.PMTrue Then

                    result = gPMConstants.PMEReturnCode.PMError

                    ' Log Error.
                    Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                    oDict.Add("v_sDocTemplateCode", v_sDocTemplateCode)
                    oDict.Add("v_lInsuranceFileCnt", v_lInsuranceFileCnt)
                    oDict.Add("v_lInsuranceFolderCnt", v_lInsuranceFolderCnt)
                    oDict.Add("v_lPartyCnt", v_lPartyCnt)
                    oDict.Add("v_lClaimCnt", v_lClaimCnt)
                    gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to produce document for template code" & v_sDocTemplateCode, vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=New Exception(Information.Err().Description), oDicParms:=oDict)
                End If

            Else

                result = gPMConstants.PMEReturnCode.PMError

                ' Log Error.
                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("v_sDocTemplateCode", v_sDocTemplateCode)
                oDict.Add("v_lInsuranceFileCnt", v_lInsuranceFileCnt)
                oDict.Add("v_lInsuranceFolderCnt", v_lInsuranceFolderCnt)
                oDict.Add("v_lPartyCnt", v_lPartyCnt)
                oDict.Add("v_lClaimCnt", v_lClaimCnt)
                gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to get doc template details for code" & v_sDocTemplateCode, vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, oDicParms:=oDict)

            End If

        End If

        Return result

    End Function


    ' ***************************************************************** '
    ' Name: GetDocTemplateDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 15-10-2003 : Continuation Tasks
    ' ***************************************************************** '
    Private Function GetDocTemplateDetails(ByVal v_sTemplateCode As String, ByVal v_vInsuranceFileCnt As Object, ByRef r_lTemplateId As Integer, ByRef r_lTemplateTypeId As Integer, ByRef r_sDocDescription As String) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "GetDocTemplateDetails"



        result = gPMConstants.PMEReturnCode.PMTrue

        ' if we dont already have an instance of the doc template find component
        If m_oFindDocTemplate Is Nothing Then

            ' create one
            If gPMComponentServices.CreateBusinessObject(r_oObject:=m_oFindDocTemplate, v_sClassName:="bSIRFindDocTemplate.Form", v_sCallingAppName:="", v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase) <> gPMConstants.PMEReturnCode.PMTrue Then
                ' if it fails log error
                m_oFindDocTemplate = Nothing

                result = gPMConstants.PMEReturnCode.PMFalse

                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("v_sTemplateCode", v_sTemplateCode)
                oDict.Add("v_vInsuranceFileCnt", v_vInsuranceFileCnt)
                oDict.Add("r_lTemplateId", r_lTemplateId)
                oDict.Add("r_lTemplateTypeId", r_lTemplateTypeId)
                gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to get instance of bSIRFindDocTemplate.Form", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, oDicParms:=oDict)

            End If

        End If

        ' if we have an instance of the doc template find component
        If Not (m_oFindDocTemplate Is Nothing) Then

            ' get the specified templates details

            If m_oFindDocTemplate.GetAvailableTemplate(v_sTemplateCode:=v_sTemplateCode, r_lTemplateId:=r_lTemplateId, r_lTemplateTypeId:=r_lTemplateTypeId, r_sDocDescription:=r_sDocDescription, v_vInsuranceFileCnt:=v_vInsuranceFileCnt, v_vEffectiveDate:=DateTime.Now) <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse

                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("v_sTemplateCode", v_sTemplateCode)
                oDict.Add("v_vInsuranceFileCnt", v_vInsuranceFileCnt)
                oDict.Add("r_lTemplateId", r_lTemplateId)
                oDict.Add("r_lTemplateTypeId", r_lTemplateTypeId)
                gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to retrieve document template details for code:" & v_sTemplateCode, vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, oDicParms:=oDict)

            End If

        End If

        Return result

    End Function


    ' ***************************************************************** '
    ' Name: DeleteStepInstance
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 22-10-2003 : Continuation Tasks
    ' ***************************************************************** '
    Public Function DeleteStepInstance(ByVal v_lStepInstanceCnt As Integer) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "DeleteStepInstance"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = CType(AddInputParameter(v_sName:="pmwrk_workflow_step_instance_cnt", v_vValue:=v_lStepInstanceCnt, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' Execute Action Query
            If m_oDatabase.SQLAction(sSQL:=ACDeleteStepInstanceSQL, sSQLName:=ACDeleteStepInstanceName, bStoredProcedure:=True) <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse

                '******************************
                ' Log Error.
                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("v_lStepInstanceCnt", v_lStepInstanceCnt)
                gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to delete workflow information for step instance: " & CStr(v_lStepInstanceCnt), vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=New Exception(Information.Err().Description), oDicParms:=oDict)
                '******************************

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error.
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_lStepInstanceCnt", v_lStepInstanceCnt)
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
    Private Function AddInputParameter(ByVal v_sName As String, ByVal v_vValue As Object, ByVal v_iType As Integer) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "AddInputParameter"



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Add Parameter to database object

        If m_oDatabase.Parameters.Add(sName:=v_sName, vValue:=CStr(v_vValue), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_iType) <> gPMConstants.PMEReturnCode.PMTrue Then

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=New Exception(Information.Err().Description))

        End If

        Return result

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
    'UPGRADE_NOTE: (7001) The following declaration (LogMsg) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub LogMsg(ByVal v_sMsg As String, ByVal v_sMethod As String)
    '
    'Const sFunctionName As String = "LogMsg"
    '
    'Try 
    '
    'gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=v_sMethod & ":" & v_sMsg, vApp:=ACApp, vClass:=ACClass, vMethod:=v_sMethod)
    '
    'Catch excep As System.Exception
    '
    '
    '
    '******************************
    ' Log Error.
    'gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
    '******************************
    '
    'Exit Sub
    '
    'End Try
    '
    'End Sub

    ' ***************************************************************** '
    ' STANDARD METHODS
    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByRef sUserName As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer




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

            m_oLookup = New bPMLookup.Business
            m_lReturn = CType(m_oLookup.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

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
                m_oLookup = Nothing
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
            m_lReturn = m_oDatabase.SQLBeginTrans()

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
        'Const sFunctionName As String = "CommitTrans"          ''Unused Local Variable

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLCommitTrans()

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
        'Const sFunctionName As String = "RollbackTrans"        ''Unused Local Variable

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLRollbackTrans()

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
