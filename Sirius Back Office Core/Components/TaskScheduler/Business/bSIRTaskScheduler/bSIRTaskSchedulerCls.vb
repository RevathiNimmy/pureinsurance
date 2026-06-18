Option Strict Off
Option Explicit On
Imports System.Data
Imports System.Data.SqlClient
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business
    Implements IDisposable

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
    Private m_oTaskScheduler As bSIRTaskScheduler.Business

    ' Database Class (Private)

    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As gPMConstants.PMEProcessMode
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_lPMAuthorityLevel As Integer

    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        End Get
    End Property

    Public WriteOnly Property PMAuthorityLevel() As Integer
        Set(ByVal Value As Integer)
            m_lPMAuthorityLevel = Value
        End Set
    End Property


    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this object.
    ' ***************************************************************** '
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long




        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

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


            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set Username and Password
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now
            ' Create TaskScheduler Collection
            m_oTaskScheduler = New bSIRTaskScheduler.Business()

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
    ' Description: Entry point for any termination code for this object.
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
                End If
                m_oDatabase = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: SetProcessModes (Standard Method)
    '
    ' Description: Set the optional process modes.
    ' ***************************************************************** '
    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the process modes to the property members.

            If Not Informations.IsNothing(vTask) Then

                m_iTask = CInt(vTask)
            End If


            If Not Informations.IsNothing(vNavigate) Then

                m_lNavigate = CInt(vNavigate)
            End If


            If Not Informations.IsNothing(vProcessMode) Then

                m_lProcessMode = CType(CInt(vProcessMode), gPMConstants.PMEProcessMode)
            End If


            If Not Informations.IsNothing(vTransactionType) Then

                m_sTransactionType = CStr(vTransactionType)
            End If


            If Not Informations.IsNothing(vEffectiveDate) Then

                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function GetBatchProcess(ByVal v_vbatchprocesses_list_id As Integer, ByRef m_vParameters(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "GetBatchProcess"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "batchProcesses_list_id", v_vbatchprocesses_list_id, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, True)

            ' Call sql

            lReturn = m_oDatabase.SQLAction(sSQL:=kSelScheduledBatchSQL, sSQLName:=kSelScheduledBatchName, bStoredProcedure:=kSelScheduledBatchStored)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLAction", "Unable to fetch Batch scheduled detail")
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
            '		Return result
            '		Resume 

            '		Return result
        End Try
        Return result
    End Function

    Public Function GetBatchFrequencyParameters(ByVal v_vbatch_scheduler_id As Integer, ByRef m_vfrequencyParameters(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "GetBatchFrequencyParameters"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "batch_scheduler_id", v_vbatch_scheduler_id, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, True)

            ' Call sql

            lReturn = m_oDatabase.SQLSelect(sSQL:=kSelScheduledBatchFrequencySQL, sSQLName:=kSelScheduledBatchFrequencyName, bStoredProcedure:=kSelScheduledBatchFrequencyStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=m_vfrequencyParameters)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "Unable to fetch Process scheduled frequency detail")
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
            '		Return result
            '		Resume 

            '		Return result
        End Try
        Return result
    End Function

    Public Function AddBatchScheduler(ByVal v_vProcessName As String, ByVal v_vJobDescription As String, ByVal v_vFrequencyType As String, ByVal v_vFrequencySubType As String, ByVal v_sFrequencyDescription As String, ByVal v_ibatchProcessId As Integer, ByVal v_vdataTable As DataTable, ByVal v_dtParameters As DataTable, ByVal v_sBatchFileName As String) As Integer

        Dim result As Integer = 0
        Dim bTrans As Integer
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim v_ibatch_scheduler_id As Integer



        Const kMethodName As String = "AddBatchScheduler"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Start a transaction

            lReturn = m_oDatabase.SQLBeginTrans
            If lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                bTrans = True
            Else
                gPMFunctions.RaiseError("m_oDatabase.SQLBeginTrans", "Unable to create SQL transaction")
            End If


            m_oDatabase.Parameters.Clear()
            'Add parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "batchprocesses_list_id", v_ibatchProcessId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            bPMAddParameter.AddParameterLite(m_oDatabase, "process", v_vProcessName, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "description", v_vJobDescription, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "frequency", v_vFrequencyType, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "frequencyDescription", v_sFrequencyDescription, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "batch_file_name", v_sBatchFileName, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "batch_scheduler_id", 0, gPMConstants.PMEParameterDirection.PMParamInputOutput, gPMConstants.PMEDataType.PMInteger)

            ' Call sql

            lReturn = m_oDatabase.SQLAction(sSQL:=kInsertScheduledBatchSQL, sSQLName:=kInsertScheduledBatchName, bStoredProcedure:=kInsertScheduledBatchStored)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLAction", "Unable to add Scheduler Batch details")
            End If


            v_ibatch_scheduler_id = m_oDatabase.Parameters.Item("batch_scheduler_id").Value

            If v_vdataTable.Rows.Count > 0 And v_ibatch_scheduler_id > 0 Then

                lReturn = CType(AddScheduleProcessParametersDetails(v_ibatch_processlist_id:=v_ibatchProcessId, v_ibatch_scheduler_id:=v_ibatch_scheduler_id, v_dtParameters:=v_dtParameters), gPMConstants.PMEReturnCode)

                ' Update Scheduled Batch Details

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("AddScheduleProcessParametersDetails", "Unable to add Scheduled Process Parameters Details")
                End If

                lReturn = CType(AddScheduleBatchDetails(v_ibatch_scheduler_id:=v_ibatch_scheduler_id, v_vdataTable:=v_vdataTable), gPMConstants.PMEReturnCode)

                ' Update Scheduled Batch Details

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("AddScheduleBatchDetails", "Unable to add Scheduled Batch Details")
                End If

            End If

            ' Commit the transaction

            lReturn = m_oDatabase.SQLCommitTrans
            If lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                bTrans = False
            Else
                gPMFunctions.RaiseError("m_oDatabase.SQLCommitTrans", "Unable to commit SQL transaction")
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
            ' If we are in a transaction when we get here roll it back!
            If bTrans Then

                m_oDatabase.SQLRollbackTrans()
            End If

            '		Return result
            '		Resume 

            '		Return result
        End Try
        Return result
    End Function

    Public Function AddScheduleProcessParametersDetails(ByVal v_ibatch_processlist_id As Integer, ByVal v_ibatch_scheduler_id As Integer, ByVal v_dtParameters As DataTable) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "AddScheduleProcessParametersDetails"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_oDatabase.Parameters.Clear()
            Using cmd As New SqlCommand(kInsertScheduledProcessParametersSQL)
                cmd.Parameters.AddWithValue("@batchprocesses_list_id", v_ibatch_processlist_id)
                cmd.Parameters.AddWithValue("@batch_scheduler_id", v_ibatch_scheduler_id)
                cmd.Parameters.AddWithValue("@tblProcessParameters", v_dtParameters)
                cmd.CommandType = CommandType.StoredProcedure

                'Execute SQL Statement
                lReturn = m_oDatabase.ExecuteNonQuery(cmd)
                'If lReturn <> PMEReturnCode.PMTrue Then
                '    RaiseError(ACClass, "GetTableName Failed", PMELogLevel.PMLogError)
                '    Return lReturn
                'End If
            End Using

            'Add parameters
            '     bPMAddParameter.AddParameterLite(m_oDatabase, "batch_scheduler_id", v_ibatch_scheduler_id, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            '    bPMAddParameter.AddParameterLite(m_oDatabase, "tblBatchFrequency", v_vdataTable, gPMConstants.PMEParameterDirection.PMParamInput, PMEDataType.PMDataTable)

            ' Call sql

            'lReturn = m_oDatabase.SQLAction(sSQL:=kInsertScheduledBatchDetailsSQL, sSQLName:=kInsertScheduledBatchDetailsName, bStoredProcedure:=kInsertScheduledBatchDetailsStored)
            'If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '''''gPMFunctions.RaiseError("m_oDatabase.SQLAction", "Unable to add Scheduler Batch details")
            'End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
            ' If we are in a transaction when we get here roll it back!

            '		Return result
            '		Resume 

            '		Return result
        End Try
        Return result
    End Function

    Public Function AddScheduleBatchDetails(ByVal v_ibatch_scheduler_id As Integer, ByVal v_vdataTable As DataTable) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "AddSchedulerBatchDetails"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_oDatabase.Parameters.Clear()
            Using cmd As New SqlCommand(kInsertScheduledBatchDetailsSQL)
                cmd.Parameters.AddWithValue("@batch_scheduler_id", v_ibatch_scheduler_id)
                cmd.Parameters.AddWithValue("@tblBatchFrequency", v_vdataTable)
                cmd.CommandType = CommandType.StoredProcedure

                'Execute SQL Statement
                lReturn = m_oDatabase.ExecuteNonQuery(cmd)
                ' If lReturn <> PMEReturnCode.PMTrue Then
                'RaiseError(ACClass, "GetTableName Failed", PMELogLevel.PMLogError)
                'Return lReturn
                'End If
            End Using

            'Add parameters
            '     bPMAddParameter.AddParameterLite(m_oDatabase, "batch_scheduler_id", v_ibatch_scheduler_id, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            '    bPMAddParameter.AddParameterLite(m_oDatabase, "tblBatchFrequency", v_vdataTable, gPMConstants.PMEParameterDirection.PMParamInput, PMEDataType.PMDataTable)

            ' Call sql

            'lReturn = m_oDatabase.SQLAction(sSQL:=kInsertScheduledBatchDetailsSQL, sSQLName:=kInsertScheduledBatchDetailsName, bStoredProcedure:=kInsertScheduledBatchDetailsStored)
            'If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '''''gPMFunctions.RaiseError("m_oDatabase.SQLAction", "Unable to add Scheduler Batch details")
            'End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
            ' If we are in a transaction when we get here roll it back!

            '		Return result
            '		Resume 

            '		Return result
        End Try
        Return result
    End Function

    Public Function GetCurrentBatchProcess(ByVal v_ibatchSchedulerId As Integer, ByRef r_vBatchProcesses(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "GetBatchProcesses"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            'If (batchProcessId > 0) Then
            bPMAddParameter.AddParameterLite(m_oDatabase, "batch_scheduler_id", v_ibatchSchedulerId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, True)
            'Else
            'm_oDatabase.Parameters.Clear()
            'End If

            ' Call sql
            lReturn = m_oDatabase.SQLSelect(sSQL:=kSelScheduledProcessSQL, sSQLName:=kSelScheduledProcessName, bStoredProcedure:=kSelScheduledProcessStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vBatchProcesses)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "Unable to get process for batch")
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
            '        Return result
            '        Resume

            '        Return result
        End Try
        Return result
    End Function


    Public Function UpdateBatchProcessesRecords(ByVal v_vProcessName As String, ByVal v_vJobDescription As String, ByVal v_vFrequencyType As String, ByVal v_vFrequencySubType As String, ByVal v_sFrequencyDescription As String, ByVal v_ibatchSchedulerId As Integer, ByVal v_vdataTable As DataTable, ByVal v_dtParameters As DataTable, ByVal v_sBatchFileName As String) As Integer

        Dim result As Integer = 0
        Dim bTrans As Integer
        Dim lReturn As gPMConstants.PMEReturnCode
        ' Dim v_ibatch_scheduler_id As Integer



        Const kMethodName As String = "UpdateBatchProcessesRecords"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Start a transaction

            lReturn = m_oDatabase.SQLBeginTrans
            If lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                bTrans = True
            Else
                gPMFunctions.RaiseError("m_oDatabase.SQLBeginTrans", "Unable to create SQL transaction")
            End If


            m_oDatabase.Parameters.Clear()
            'Add parameters

            bPMAddParameter.AddParameterLite(m_oDatabase, "process", v_vProcessName, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "description", v_vJobDescription, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "frequency", v_vFrequencyType, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "frequencyDescription", v_sFrequencyDescription, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "batch_scheduler_id", v_ibatchSchedulerId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            bPMAddParameter.AddParameterLite(m_oDatabase, "batch_file_name", v_sBatchFileName, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

            ' Call sql

            lReturn = m_oDatabase.SQLAction(sSQL:=kUpdateScheduledProcessSQL, sSQLName:=kUpdateScheduledProcessName, bStoredProcedure:=kUpdateScheduledProcessStored)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLAction", "Unable to add Scheduler Batch details")
            End If

            ' v_ibatch_scheduler_id = m_oDatabase.Parameters.Item("batch_scheduler_id").Value

            'If v_vdataTable.Rows.Count > 0 Then

            lReturn = CType(UpdateBatchProcessesParameterRecords(v_ibatch_scheduler_id:=v_ibatchSchedulerId, v_dtParameters:=v_dtParameters, v_vdataTable:=v_vdataTable), gPMConstants.PMEReturnCode)

            ' Update Scheduled Batch Details

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("UpdateBatchProcessesParameterRecords", "Unable to update Scheduled Process Parameters Details")
            End If



            'End If

            ' Commit the transaction

            lReturn = m_oDatabase.SQLCommitTrans
            If lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                bTrans = False
            Else
                gPMFunctions.RaiseError("m_oDatabase.SQLCommitTrans", "Unable to commit SQL transaction")
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
            ' If we are in a transaction when we get here roll it back!
            If bTrans Then

                m_oDatabase.SQLRollbackTrans()
            End If

            '		Return result
            '		Resume 

            '		Return result
        End Try
        Return result
    End Function

    Public Function UpdateBatchProcessesParameterRecords(ByVal v_ibatch_scheduler_id As Integer, ByVal v_dtParameters As DataTable, ByVal v_vdataTable As DataTable) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "UpdateBatchProcessesParameterRecords"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_oDatabase.Parameters.Clear()
            Using cmd As New SqlCommand(kUpdateScheduledParametersSQL)

                cmd.Parameters.AddWithValue("@batch_scheduler_id", v_ibatch_scheduler_id)
                cmd.Parameters.AddWithValue("@tblProcessParameters", v_dtParameters)
                cmd.Parameters.AddWithValue("@tblBatchFrequency", v_vdataTable)
                cmd.CommandType = CommandType.StoredProcedure

                'Execute SQL Statement
                lReturn = m_oDatabase.ExecuteNonQuery(cmd)
                'If lReturn <> PMEReturnCode.PMTrue Then
                '    RaiseError(ACClass, "GetTableName Failed", PMELogLevel.PMLogError)
                '    Return lReturn
                'End If
            End Using



            'lReturn = m_oDatabase.SQLAction(sSQL:=kInsertScheduledBatchDetailsSQL, sSQLName:=kInsertScheduledBatchDetailsName, bStoredProcedure:=kInsertScheduledBatchDetailsStored)
            'If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '''''gPMFunctions.RaiseError("m_oDatabase.SQLAction", "Unable to add Scheduler Batch details")
            'End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
            ' If we are in a transaction when we get here roll it back!

            '		Return result
            '		Resume 

            '		Return result
        End Try
        Return result
    End Function


    Public Sub New()
        MyBase.New()
        Exit Sub
    End Sub


End Class
