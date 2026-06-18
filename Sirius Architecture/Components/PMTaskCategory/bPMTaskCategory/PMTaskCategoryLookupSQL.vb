Option Strict Off
Option Explicit On
Module PMTaskLookupSQL
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    '

    ' ***************************************************************** '
    ' Class Name: PMTaskLookupSQL
    '
    ' Date: 22 October 1998
    '
    ' Description: Contains the SQL/Stored Procedures used by the
    '              Lookup class.
    '
    ' Edit History:
    ' ***************************************************************** '

    'SQL Statements

    ' Task Category SQL
    Public Const ACTaskCategoryLookupStored As Boolean = True
    Public Const ACTaskCategoryLookupName As String = "TaskCategoryLicence"
    Public Const ACTaskCategoryLookupSQL As String = "spu_pmwrk_inst_category_sel"
End Module