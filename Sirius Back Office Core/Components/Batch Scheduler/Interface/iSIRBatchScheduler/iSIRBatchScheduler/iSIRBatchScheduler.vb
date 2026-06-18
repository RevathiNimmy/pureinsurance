Option Strict Off
Option Explicit On
Imports System
Module MainModule
    ' ***************************************************************** '
    ' Module Name: MainModule
    ' ***************************************************************** '

    ' Main public constant for all functions to identify which application this is.
    Public Const kApp As String = "iSIRBatchScheduler"

    ' Constant for the functions to identify which class this is.
    Private Const kClass As String = "MainModule"

    'Get Res Data
    Public Const kInterfaceCaption As Integer = 100 'Batch Scheduled

    ' Buttons
    Public Const kViewButton As Integer = 101
    Public Const kNewButton As Integer = 102
    Public Const kDeleteButton As Integer = 103
    Public Const kOKButton As Integer = 104
    Public Const kEditButton As Integer = 105
    Public Const kCancelButton As Integer = 106
    Public Const kInterfaceCaptionDetail As Integer = 107

    'List
    Public Const kListTitle1 As Integer = 110 'Process
    Public Const kListTitle2 As Integer = 111 'Description
    Public Const kListTitle3 As Integer = 112 'Frequency
    Public Const kListTitle4 As Integer = 113 'Status

    ' Public Const ACListDetailTitle1 As Integer = 113 'Automatic
    Public Const ACListDetailTitle2 As Integer = 114 'Parameter ID
	Public Const ACListDetailTitle3 As Integer = 115 'Parameter Name
	Public Const ACListDetailTitle4 As Integer = 116 'Parameter Type
	Public Const ACListDetailTitle5 As Integer = 117 'Parameter Value

    ''''''''''''''

    ' ***************************************************************** '
    '                         Batch Process ARRAY
    ' ***************************************************************** '
    Public Enum BatchSchedulerEnum
        DBMBatchSchedulerID
        DBMBatchProcessID
        DBMProcess
        DBMDescription
        DBMFrequencyDescription
        DBMStatus
        DBMProcessSelected
        DBMProcessDescription
        DBMBatchFileName
    End Enum

    'Public DBMMax As BatchSchedulerEnum = BatchSchedulerEnum.DBMSeprateBy

    ' ***************************************************************** '
    '                      Report Scheduler Detail ARRAY
    ' ***************************************************************** '
    Public Enum BatchSchedulerDetailEnum
        DBMReportSchedulerParameterID
        DBMReportSchedulerDetailID
        DBMParameterName
        DBMDefaultValue
        DBMDataType
        DBMPrompt
        DBMCurrentIDValue
        DBMPartySearch
        DBMEmpty
        DBMIsAutomatic
    End Enum

    Public Enum BatchProcessParmeterEnum
        'DBMBatchSchedulerID
        ' DBMBatchProcessID
        DBMParameterName
        DBMParameterValue
        DBMProcess
        DBMFrequency

    End Enum






    ' ***************************************************************** '
    '                        GLOBAL VARIABLES
    ' ***************************************************************** '
    ' Public source and language ID's from the Object Manager.
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_iSourceID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iLanguageID As Integer
	
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
End Module