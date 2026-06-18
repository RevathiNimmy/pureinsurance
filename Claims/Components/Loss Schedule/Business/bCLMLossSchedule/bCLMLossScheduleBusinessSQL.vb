Option Strict Off
Option Explicit On
Module BusinessSQL
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    '

    ' ***************************************************************** '
    ' Class Name: BusinessSQL
    '
    ' Date: 17092002
    '
    ' Description: Contains the SQL Statements required by the
    '              bCLMLossSchedule.Business class.
    '
    ' Edit History:
    ' ***************************************************************** '

    'SQL Statements

    Public Const ACGetLossScheduleTypesName As String = "Get Loss Schedule Types"
    Public Const ACGetLossScheduleTypesSQL As String = "{call spu_Get_Loss_Schedule_Type}"

    Public Const ACGetItemDetailsName As String = "Get Item Details"
    Public Const ACGetItemDetailsSQL As String = "{call spu_get_item_details(?)}"
End Module