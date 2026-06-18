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
    ' Date: 11/07/1997
    '
    ' Description: Contains the SQL Statements required by the
    '              bPMCurrency.Form class.
    '
    ' Edit History:
    ' ***************************************************************** '

    'SQL Statements
    'developer guide no.39
    'start
    ' Select Currency SQL
    Public Const ACGetDetailsStored As Boolean = True
    Public Const ACGetDetailsName As String = "SelectCurrency"
    Public Const ACGetDetailsSQL As String = "spu_currency_sel"

    ' Select Currency by ISO code SQL
    Public Const ACGetDetailsByCodeStored As Boolean = True
    Public Const ACGetDetailsByCodeName As String = "SelectCurrency"
    Public Const ACGetDetailsByCodeSQL As String = "spu_currency_sel_by_code"

    ' Select All Currency SQL
    Public Const ACGetAllDetailsStored As Boolean = True
    Public Const ACGetAllDetailsName As String = "SelectAllCurrency"
    Public Const ACGetAllDetailsSQL As String = "spu_currency_selall"

    ' Check ID SQL
    Public Const ACCheckIDStored As Boolean = True
    Public Const ACCheckIDName As String = "CheckCurrencyID"
    Public Const ACCheckIDSQL As String = "spu_currency_check"

    ' Add Currency SQL
    Public Const ACAddStored As Boolean = True
    Public Const ACAddName As String = "AddCurrency"
    Public Const ACAddSQL As String = "spu_currency_add"

    ' Update Currency SQL
    Public Const ACUpdateStored As Boolean = True
    Public Const ACUpdateName As String = "UpdateCurrency"
    Public Const ACUpdateSQL As String = "spu_currency_upd"
    'end
End Module