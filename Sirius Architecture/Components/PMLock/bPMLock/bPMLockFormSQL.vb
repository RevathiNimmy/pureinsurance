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
    ' Date: 01/07/1997
    '
    ' Description: Contains the SQL Statements required by the
    '              bPMLock.Form class.
    '
    ' Edit History: Created by TF 01/07/1997
    ' ***************************************************************** '

    'SQL Statements

    ' Example select using embedded SQL
    ' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
    ' Public Const ACSelectStored = False
    ' Public Const ACSelectName = "SelectRisk"
    ' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"

    ' Delete PMLockAll SQL
    'developer guide no.39
    Public Const ACDeletePMLockAllStored As Boolean = True
    Public Const ACDeletePMLockAllName As String = "DeletePMLockAll"
    Public Const ACDeletePMLockAllSQL As String = "spu_delete_pmlock_all"

    ' Delete PMLockKey SQL
    Public Const ACDeletePMLockKeyStored As Boolean = True
    Public Const ACDeletePMLockKeyName As String = "DeletePMLockKey"
    Public Const ACDeletePMLockKeySQL As String = "spu_delete_pmlock_key"

    ' Delete PMLockUser SQL
    Public Const ACDeletePMLockUserStored As Boolean = True
    Public Const ACDeletePMLockUserName As String = "DeletePMLockUser"
    Public Const ACDeletePMLockUserSQL As String = "spu_delete_pmlock_user"

    ' Select PMLockAll SQL
    Public Const ACSelectPMLockAllStored As Boolean = True
    Public Const ACSelectPMLockAllName As String = "SelectPMLockAll"
    Public Const ACSelectPMLockAllSQL As String = "spu_select_all_pmlock"
End Module