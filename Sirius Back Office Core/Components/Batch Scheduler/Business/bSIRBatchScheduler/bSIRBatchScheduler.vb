Option Strict Off
Option Explicit On
Imports System
Module ModuleBatch

    ' Main public constant for all functions to identify which application this is.
    Public Const ACApp As String = "bSIRBatchScheduler"

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "MainModule"

    Public Enum BatchProcessParmeterEnum
        'DBMBatchSchedulerID
        ' DBMBatchProcessID
        DBMParameterName
        DBMParameterValue
        DBMProcess
        DBMFrequency

    End Enum

End Module
