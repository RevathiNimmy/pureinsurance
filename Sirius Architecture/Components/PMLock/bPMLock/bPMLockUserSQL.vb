Option Strict Off
Option Explicit On
Module UserSQL
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    '

    ' ***************************************************************** '
    ' Class Name: UserSQL
    '
    ' Date: 30/06/1997
    '
    ' Description: Contains the SQL Statements required by the
    '              bPMLock.User class.
    '
    ' Edit History: Created by TF 30/06/1997
    ' ***************************************************************** '

    'SQL Statements

    ' Example select using embedded SQL
    ' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
    ' Public Const ACSelectStored = False
    ' Public Const ACSelectName = "SelectRisk"
    ' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"

    ' Add PMLock SQL
    'developer guide no.39
    Public Const ACAddPMLockStored As Boolean = True
    Public Const ACAddPMLockName As String = "AddPMLock"
    Public Const ACAddPMLockSQL As String = "spu_add_pmlock"

    'this check if the record if the same user has the record locked
    Public Const ACAddPMLockStored2 As Boolean = True
    Public Const ACAddPMLockName2 As String = "AddPMLock2"
    Public Const ACAddPMLockSQL2 As String = "spu_add_pmlock2"

    ' Delete PMLock SQL
    Public Const ACDeletePMLockStored As Boolean = True
    Public Const ACDeletePMLockName As String = "DeletePMLock"
    Public Const ACDeletePMLockSQL As String = "spu_delete_pmlock"


    ' Check Timestamp SQL
    Public Const ACCheckTSStored As Boolean = True
    Public Const ACCheckTSName As String = "CheckPMLockTimestamp"
    Public Const ACCheckTSSQL As String = "spu_pmlock_last_unlock_check"

    ' Get Timestamp SQL
    Public Const ACGetTSStored As Boolean = True
    Public Const ACGetTSName As String = "GetPMLockTimestamp"
    Public Const ACGetTSSQL As String = "spu_pmlock_last_unlock_sel"
End Module