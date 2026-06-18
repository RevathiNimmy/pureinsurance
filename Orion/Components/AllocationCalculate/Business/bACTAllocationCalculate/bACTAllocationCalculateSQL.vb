Option Strict Off
Option Explicit On
Module AllocateSQL
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    '

    ' ***************************************************************** '
    ' Class Name: FormSQL
    '
    ' Date: 11/08/1997
    '
    ' Description: Contains the SQL Statements required by the
    '              bACTAllocationCalculate.Form class.
    '
    ' Edit History:
    ' ***************************************************************** '

    'SQL Statements

    ' Select Allocation SQL
    Public Const ACGetDetailsStored As Boolean = True
    Public Const ACGetDetailsName As String = "SelectAllocation"
    'developer guide no 39
    Public Const ACGetDetailsSQL As String = "spu_ACT_Select_Allocation"

    ' Select All Allocation SQL
    Public Const ACGetAllDetailsStored As Boolean = True
    Public Const ACGetAllDetailsName As String = "SelectAllAllocation"
    'developer guide no 39

    Public Const ACGetAllDetailsSQL As String = "spu_ACT_selall_Allocation"

    ' Check ID SQL
    Public Const ACCheckIDStored As Boolean = True
    Public Const ACCheckIDName As String = "CheckAllocationID"
    'developer guide no 39
    Public Const ACCheckIDSQL As String = "spu_ACT_check_Allocation"

    ' Add Allocation SQL
    Public Const ACAddStored As Boolean = True
    Public Const ACAddName As String = "AddAllocation"
    'developer guide no 39
    Public Const ACAddSQL As String = "spu_ACT_add_Allocation"

    ' Delete Allocation SQL
    Public Const ACDeleteStored As Boolean = True
    Public Const ACDeleteName As String = "DeleteAllocation"
    'developer guide no 39
    Public Const ACDeleteSQL As String = "spu_ACT_delete_Allocation"

    ' Update Allocation SQL
    Public Const ACUpdateStored As Boolean = True
    Public Const ACUpdateName As String = "UpdateAllocation"
    'developer guide no 39
    Public Const ACUpdateSQL As String = "spu_ACT_update_Allocation"
    Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Module