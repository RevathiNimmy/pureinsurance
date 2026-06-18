Option Strict Off
Option Explicit On
Module FormSQL
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    '

    ' ***************************************************************** '
    ' Class Name: FormSQL
    '
    ' Date: 31/07/1997
    '
    ' Description: Contains the SQL Statements required by the
    '              bACTPeriod.Form class.
    '
    ' Edit History:
    ' ***************************************************************** '

    'SQL Statements

    ' Example select using embedded SQL
    ' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
    ' Public Const ACSelectStored = False
    ' Public Const ACSelectName = "SelectRisk"
    ' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"

    ' Select Period SQL
    Public Const ACGetDetailsStored As Boolean = True
    Public Const ACGetDetailsName As String = "SelectAllPeriod"
    'developer guide no 39
    Public Const ACGetDetailsSQL As String = "spu_ACT_Select_Period"

    ' Select All Period SQL
    Public Const ACGetAllDetailsStored As Boolean = True
    Public Const ACGetAllDetailsName As String = "SelectAllPeriod"
    'developer guide no 39
    Public Const ACGetAllDetailsSQL As String = "spu_ACT_SelAll_Period"

    ' Select Period Year Names only SQL
    Public Const ACGetPeriodYearsStored As Boolean = True
    Public Const ACGetPeriodYearsName As String = "SelectPeriodYears"
    'developer guide no 39
    Public Const ACGetPeriodYearsSQL As String = "spu_ACT_SelAll_PeriodYear"

    ' Select the latest Period End Date SQL
    Public Const ACGetPeriodLastDateStored As Boolean = True
    Public Const ACGetPeriodLastDateName As String = "SelectPeriodLastDate"
    'developer guide no 39
    Public Const ACGetPeriodLastDateSQL As String = "spu_ACT_Select_Period_LastDate"

    ' Check ID SQL
    Public Const ACCheckIDStored As Boolean = True
    Public Const ACCheckIDName As String = "CheckPeriodID"
    'developer guide no 39
    Public Const ACCheckIDSQL As String = "spu_ACT_Check_Period"

    ' Add Period SQL
    Public Const ACAddStored As Boolean = True
    Public Const ACAddName As String = "AddPeriod"
    'developer guide no 39
    Public Const ACAddSQL As String = "spu_ACT_Add_Period"

    ' Delete Period SQL
    Public Const ACDeleteStored As Boolean = True
    Public Const ACDeleteName As String = "DeletePeriod"
    'developer guide no 39
    Public Const ACDeleteSQL As String = "spu_ACT_Delete_Period"

    ' Update Period SQL
    Public Const ACUpdateStored As Boolean = True
    Public Const ACUpdateName As String = "UpdatePeriod"
    'developer guide no 39
    Public Const ACUpdateSQL As String = "spu_ACT_Update_Period"

    ' Get Period for given date SQL
    Public Const ACGetPeriodForDateStored As Boolean = True
    Public Const ACGetPeriodForDateName As String = "UpdatePeriod"
    Public Const ACGetPeriodForDateSQL As String = "spu_ACT_Do_GetPeriodForDate"

    ' Get next period id SQL
    Public Const ACGetNextPeriodIDStored As Boolean = True
    Public Const ACGetNextPeriodIDName As String = "GetNextPeriodID"
    'developer guide no 39
    Public Const ACGetNextPeriodIDPeriodStoredSQL As String = "spu_ACT_Get_Next_Period_Id"

    ' Get Previous period id SQL
    Public Const ACGetPreviousPeriodIDStored As Boolean = True
    Public Const ACGetPreviousPeriodIDName As String = "GetPreviousPeriodID"
    'developer guide no 39
    Public Const ACGetPreviousPeriodIDPeriodStoredSQL As String = "spu_ACT_Get_Previous_Period_Id"

    ' Get Period Start Date SQL
    Public Const ACGetPeriodStartDateStored As Boolean = True
    Public Const ACGetPeriodStartDateName As String = "GetPeriodStartDate"
    'developer guide no 39
    Public Const ACGetPeriodStartDateStoredSQL As String = "spu_ACT_Get_Period_Start_Date"

    ' DD 31/07/2002
    ' Get Current Period Details SQL
    Public Const ACGetCurrentPeriodStored As Boolean = True
    Public Const ACGetCurrentPeriodName As String = "GetCurrentPeriod"
    'developer guide no 39
    Public Const ACGetCurrentPeriodSQL As String = "spu_ACT_Get_Current_Period"

    ' DD 31/07/2002
    ' Get Current Period Details SQL
    Public Const ACGetUniqueYearsStored As Boolean = True
    Public Const ACGetUniqueYearsName As String = "GetUniqueYears"
    'developer guide no 39
    Public Const ACGetUniqueYearsSQL As String = "spu_ACT_Get_Unique_Years"

    'DD 02/08/2002
    'Sub Branches
    Public Const ACGetSubBranchStored As Boolean = True
    Public Const ACGetSubBranchName As String = "SelectSubBranches"
    'developer guide no 39
    Public Const ACGetSubBranchSQL As String = "spu_sub_branch_sel"

    'ECK PN4460 02/06/2003
    'Sub Branches
    Public Const ACGetSubBranchDefaultStored As Boolean = True
    Public Const ACGetSubBranchDefaultName As String = "GetDefaultSubBranch"
    'developer guide no 39
    Public Const ACGetSubBranchDefaultSQL As String = "spu_sub_branch_default"


    Public Const ACGetLatestUsedPeriodStored As Boolean = True
    Public Const ACGetLatestUsedPeriodName As String = "GetLatestUsedPeriod"
    'developer guide no 39
    Public Const ACGetLatestUsedPeriodSQL As String = "spu_ACT_Get_Latest_Used_Period"
    Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Module