Option Strict Off
Option Explicit On
Module SQL
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    '

    ' ***************************************************************** '
    ' Class Name: SQL
    '
    ' Date: 21/10/1998
    '
    ' Description: Contains the SQL Statements required by the
    '              SIRAddress class.
    '
    ' Edit History:
    ' ***************************************************************** '

    'SQL Statements

    ' Example select using embedded SQL
    ' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
    ' Public Const ACSelectStored = False
    ' Public Const ACSelectName = "SelectRisk"
    ' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"

    ' Select SIRAddress SQL
    Public Const ACSelectSingleStored As Boolean = True
    Public Const ACSelectSingleName As String = "SelectSingleSIRAddress"
    'developer guide no. 39 (guide)
    Public Const ACSelectSingleSQL As String = "spe_Address_sel"

    ' Add SIRAddress SQL
    Public Const ACAddStored As Boolean = True
    Public Const ACAddName As String = "AddSIRAddress"
    'developer guide no. 39(guide)
    Public Const ACAddSQL As String = "spe_Address_add"

    ' Delete SIRAddress SQL
    Public Const ACDeleteStored As Boolean = True
    Public Const ACDeleteName As String = "DeleteSIRAddress"
    'developer guide no. 39(guide)
    Public Const ACDeleteSQL As String = "spe_Address_del"

    ' Update SIRAddress SQL
    Public Const ACUpdateStored As Boolean = True
    Public Const ACUpdateName As String = "UpdateSIRAddress"
    'developer guide no. 39(guide)
    Public Const ACUpdateSQL As String = "spe_Address_upd"

    ' Check Before Add SIRAddress SQL
    Public Const ACCheckStored As Boolean = True
    Public Const ACCheckName As String = "CheckSIRAddress"
    'developer guide no. 39
    Public Const ACCheckSQL As String = "spu_Address_Check"
End Module