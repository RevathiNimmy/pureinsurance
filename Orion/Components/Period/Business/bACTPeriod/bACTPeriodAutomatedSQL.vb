Option Strict Off
Option Explicit On
Module AutomatedSQL
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    '

    ' ***************************************************************** '
    ' Class Name: AutomatedSQL
    '
    ' Date: 31/07/1997
    '
    ' Description: Contains the SQL Statements required by the
    '              bACTPeriod.Automated class.
    '
    ' Edit History:
    ' ***************************************************************** '

    'SQL Statements

    ' Example select using embedded SQL
    ' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
    ' Public Const ACAutoSelectStored = False
    ' Public Const ACAutoSelectName = "SelectRisk"
    ' Public Const ACAutoSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"

    ' Select Period SQL
    Public Const ACAutoGetDetailsStored As Boolean = True
    Public Const ACAutoGetDetailsName As String = "SelectAllPeriod"
    'developer guide no 39
    Public Const ACAutoGetDetailsSQL As String = "spu_ACT_Select_Period"

    ' Select All Period SQL
    Public Const ACAutoGetAllDetailsStored As Boolean = True
    Public Const ACAutoGetAllDetailsName As String = "SelectAllPeriod"
    'developer guide no 39
    Public Const ACAutoGetAllDetailsSQL As String = "spu_ACT_SelAll_Period"

    ' Check ID SQL
    Public Const ACAutoCheckIDStored As Boolean = True
    Public Const ACAutoCheckIDName As String = "CheckPeriodID"
    'developer guide no 39
    Public Const ACAutoCheckIDSQL As String = "spu_ACT_Check_Period"

    ' Add Period SQL
    Public Const ACAutoAddStored As Boolean = True
    Public Const ACAutoAddName As String = "AddPeriod"
    'developer guide no 39
    Public Const ACAutoAddSQL As String = "spu_ACT_Add_Period"

    ' Delete Period SQL
    Public Const ACAutoDeleteStored As Boolean = True
    Public Const ACAutoDeleteName As String = "DeletePeriod"
    'developer guide no 39
    Public Const ACAutoDeleteSQL As String = "spu_ACT_Delete_Period"

    ' Update Period SQL
    Public Const ACAutoUpdateStored As Boolean = True
    Public Const ACAutoUpdateName As String = "UpdatePeriod"
    'developer guide no 39
    Public Const ACAutoUpdateSQL As String = "spu_ACT_Update_Period"
    Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Module