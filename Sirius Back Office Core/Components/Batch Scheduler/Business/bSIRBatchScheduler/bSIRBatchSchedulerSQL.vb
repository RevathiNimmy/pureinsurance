Module bSIRBatchSchedulerSQL
    Public Const kGetBatchProcessesStored As Boolean = True
    Public Const kGetBatchProcessesName As String = "Get BatchProcesses Select"
    Public Const kGetBatchProcessesSQL As String = "spu_sir_batchprocesses_sel"


    Public Const kGetScheduledBatchProcessesStored As Boolean = True
    Public Const kGetScheduledBatchProcessesName As String = "Scheduled Batch Processes Select"
    Public Const kGetScheduledBatchProcessesSQL As String = "spu_sir_scheduled_batchprocesses_sel"

    ' Delete
    Public Const kDeleteScheduledBatchProcessesStored As Boolean = True
    Public Const kDeleteScheduledBatchProcessesName As String = "Delete Batch Scheduled"
    Public Const kDeleteScheduledBatchProcessesSQL As String = "spu_sir_scheduled_batchprocesses_delete"


    Public Const kGetBatchProcessesParameterStored As Boolean = True
    Public Const kGetBatchProcessesParameterName As String = "Batch Processes Parameter Select"
    Public Const kGetBatchProcessesParameterSQL As String = "spu_sir_batchprocess_parameter_sel"

End Module
