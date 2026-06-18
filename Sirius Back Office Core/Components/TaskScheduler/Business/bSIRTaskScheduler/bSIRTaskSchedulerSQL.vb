Option Strict Off
Option Explicit On
Module BusinessSQL

    ' Insert
    Public Const kInsertScheduledBatchStored As Boolean = True
    Public Const kInsertScheduledBatchName As String = "Insert Scheduled Batch"
    Public Const kInsertScheduledBatchSQL As String = "spu_sir_scheduled_batch_add"

    Public Const kSelScheduledBatchStored As Boolean = True
    Public Const kSelScheduledBatchName As String = "Get Batch Parameters"
    Public Const kSelScheduledBatchSQL As String = "spu_sir_scheduled_batchprocessparam_sel"

    Public Const kSelScheduledBatchFrequencyStored As Boolean = True
    Public Const kSelScheduledBatchFrequencyName As String = "Get Batch Frequency Parameters"
    Public Const kSelScheduledBatchFrequencySQL As String = "spu_sir_scheduled_batchfrequencyparam_sel"



    Public Const kInsertScheduledBatchDetailsStored As Boolean = True
    Public Const kInsertScheduledBatchDetailsName As String = "Insert Scheduled Batch Details"
    Public Const kInsertScheduledBatchDetailsSQL As String = "spu_sir_scheduled_batch_add_details"

    Public Const kInsertScheduledProcessParametersStored As Boolean = True
    Public Const kInsertScheduledProcessParametersName As String = "Insert Scheduled Process Parameters"
    Public Const kInsertScheduledProcessParametersSQL As String = "spu_sir_scheduled_processparameters_add"

    Public Const kUpdateScheduledProcessStored As Boolean = True
    Public Const kUpdateScheduledProcessName As String = "Update Scheduled Processes"
    Public Const kUpdateScheduledProcessSQL As String = "spu_sir_scheduled_batchprocess_update_details"

    Public Const kUpdateScheduledParametersStored As Boolean = True
    Public Const kUpdateScheduledParametersName As String = "Update Scheduled Parameters Processes"
    Public Const kUpdateScheduledParametersSQL As String = "spu_sir_update_batchprocess_parameters_details"

    Public Const kSelScheduledProcessStored As Boolean = True
    Public Const kSelScheduledProcessName As String = "Get Process Name"
    Public Const kSelScheduledProcessSQL As String = "spu_sir_scheduled_processes_sel"



End Module
