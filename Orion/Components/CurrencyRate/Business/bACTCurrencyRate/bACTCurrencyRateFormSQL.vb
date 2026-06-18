Option Strict Off
Option Explicit On
Imports System
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
    ' Date: 24/07/1997
    '
    ' Description: Contains the SQL Statements required by the
    '              bACTCurrencyRate.Form class.
    '
    ' Edit History:
    ' ***************************************************************** '

    'SQL Statements

    ' Select All CurrencyRate SQL
    'developer guide no. 39
    Public Const ACGetAllDetailsStored As Boolean = True
    Public Const ACGetAllDetailsName As String = "SelectAllCurrencyRate"
    Public Const ACGetAllDetailsSQL As String = "spu_ACT_selall_CurrencyRate"

    ' Add CurrencyRate SQL
    Public Const ACAddStored As Boolean = True
    Public Const ACAddName As String = "AddCurrencyRate"
    Public Const ACAddSQL As String = "spu_ACT_add_CurrencyRate"

    ' Delete CurrencyRate SQL
    Public Const ACDeleteStored As Boolean = True
    Public Const ACDeleteName As String = "DeleteCurrencyRate"
    Public Const ACDeleteSQL As String = "spu_ACT_delete_CurrencyRate"

    ' Update CurrencyRate SQL
    Public Const ACUpdateStored As Boolean = True
    Public Const ACUpdateName As String = "UpdateCurrencyRate"
    Public Const ACUpdateSQL As String = "spu_ACT_update_CurrencyRate"

    ' Get Rate For Date SQL
    Public Const ACGetRateForDateStored As Boolean = True
    Public Const ACGetRateForDateName As String = "GetRateForDate"
    Public Const ACGetRateForDateSQL As String = "spu_ACT_Get_Currency_Rate"

    '  Apply Currency Rates to all branches
    Public Const ACApplyCurrencyRateToAllBranchesStored As Boolean = True
    Public Const ACApplyCurrencyRateToAllBranchesName As String = "ApplyAllBranches"
    Public Const ACApplyCurrencyRateToAllBranchesSQL As String = "spu_ACT_Apply_All_Currency_Rates"

    'GetTypeOfRates
    Public Const ACGetTypeOfRatesStored As Boolean = True
    Public Const ACGetTypeOfRatesName As String = "GetTypeOfRates"
    Public Const ACGetTypeOfRatesSQL As String = "spu_ACT_GetTypeOfRates"

    'GetTypeOfRates
    Public Const ACGetNextEffectiveDateStored As Boolean = True
    Public Const ACGetNextEffectiveDateName As String = "GetNextEffectiveDate"
    Public Const ACGetNextEffectiveDateSQL As String = "spu_ACT_GetNextEffectiveDateForRates"
    'developer guide no. 29(No Solutions)
    'Shared Sub New()
    Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Module