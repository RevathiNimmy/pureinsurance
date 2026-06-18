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
    ' Date: 21/01/1998
    '
    ' Description: Contains the SQL Statements required by the 
    '              bACTAllocation.Automated class.
    '
    ' Edit History:
    ' ***************************************************************** '

    'SQL Statements

    ' Example select using embedded SQL
    ' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
    ' Public Const ACAutoSelectStored = False
    ' Public Const ACAutoSelectName = "SelectRisk"
    ' Public Const ACAutoSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
    'developer guide no.39
    'start
    ' Select Allocation SQL
    Public Const ACAutoGetDetailsStored As Boolean = True
    Public Const ACAutoGetDetailsName As String = "SelectAllAllocation"
    Public Const ACAutoGetDetailsSQL As String = "spu_ACT_select_{NewTable}"

    ' Select All Allocation SQL
    Public Const ACAutoGetAllDetailsStored As Boolean = True
    Public Const ACAutoGetAllDetailsName As String = "SelectAllAllocation"
    Public Const ACAutoGetAllDetailsSQL As String = "spu_ACT_select_all_{NewTable}"

    ' Check ID SQL
    Public Const ACAutoCheckIDStored As Boolean = True
    Public Const ACAutoCheckIDName As String = "CheckAllocationID"
    Public Const ACAutoCheckIDSQL As String = "spu_ACT_check_{NewTable}"

    ' Add Allocation SQL
    Public Const ACAutoAddStored As Boolean = True
    Public Const ACAutoAddName As String = "AddAllocation"
    Public Const ACAutoAddSQL As String = "spu_ACT_add_{NewTable}"

    ' Delete Allocation SQL
    Public Const ACAutoDeleteStored As Boolean = True
    Public Const ACAutoDeleteName As String = "DeleteAllocation"
    Public Const ACAutoDeleteSQL As String = "spu_ACT_delete_{NewTable}"

    ' Update Allocation SQL
    Public Const ACAutoUpdateStored As Boolean = True
    Public Const ACAutoUpdateName As String = "UpdateAllocation"
    Public Const ACAutoUpdateSQL As String = "spu_ACT_update_{NewTable}"
    'end

End Module