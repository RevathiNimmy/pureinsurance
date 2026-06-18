Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
Imports Artinsoft.VB6.Gui

<System.Runtime.InteropServices.ProgId("MainModule_NET.MainModule")>
Public Module iSIRFrequencySchedulerMod
    Private Const ACClass As String = "iSIRFrequencySchedulerMod"

    Public Const ACApp As String = "iSIRFrequencyScheduler"
    <ThreadStatic()>
    Public g_oObjectManager As bObjectManager.ObjectManager
    <ThreadStatic()>
    Public m_oBusiness As Object


    Public Enum FrequencyParameterDetailEnum

        DBMBatchSchedulerID
        DBMFrequency
        DBMParameterName
        DBMCurrentIDValue
        DBMFrequencyType
    End Enum

    Public Enum EnumDaysOfWeek

        '/// <summary>Sunday</summary>
        Sunday = 0
        '/// <summary>Monday</summary>
        Monday = 1
        '/// <summary>Tuesday</summary>
        Tuesday = 2
        '/// <summary>Wednesday</summary>
        Wednesday = 3
        '/// <summary>Thursday</summary>
        Thursday = 4
        '/// <summary>Friday</summary>
        Friday = 5
        '/// <summary>Saturday</summary>
        Saturday = 6
        '/// <summary>All days</summary>
        AllDays = 7
    End Enum

End Module
