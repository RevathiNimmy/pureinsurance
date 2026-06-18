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
    ' Date: 11/07/2000
    '
    ' Description: Contains the SQL Statements required by the
    '              bSIRShortperiodRate.Business class.
    '
    ' Edit History:
    ' ***************************************************************** '

    'SQL Statements

    ' Select All SIRShortPeriodRate SQL
    Public Const ACAddDetailsStored As Boolean = True
    Public Const ACAddDetailsName As String = "UpdateShortPeriodRates"
    'developer guide no. 39
    Public Const ACAddDetailsSQL As String = "spe_Short_Period_Rates_Add"

    Public Const ACDelDetailsStored As Boolean = True
    Public Const ACDelDetailsName As String = "DeleteShortPeriodRates"
    Public Const ACDelDetailsSQL As String = "spu_Short_Period_Rates_Del"

    Public Const ACFindDetailsStored As Boolean = True
    Public Const ACFindDetailsName As String = "FindShortPeriodRates"
    Public Const ACFindDetailsSQL As String = "spu_Short_Period_Rates_Sel"
End Module