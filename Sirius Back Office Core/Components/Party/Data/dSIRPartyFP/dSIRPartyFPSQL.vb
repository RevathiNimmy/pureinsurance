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
    ' Date: 16/10/2000
    '
    ' Description: Contains the SQL Statements required by the
    '              Finance Provider class.
    '
    ' Edit History: TF161000 - Created
    ' ***************************************************************** '

    'SQL Statements

    ' Example select using embedded SQL
    ' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
    ' Public Const ACSelectStored = False
    ' Public Const ACSelectName = "SelectRisk"
    ' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"

    'Developer Guide No 39
    'Starts
    ' Select Finance Provider SQL
    Public Const ACSelectSingleStored As Boolean = True
    Public Const ACSelectSingleName As String = "SelectSinglePartyFinanceProvider"
    Public Const ACSelectSingleSQL As String = "spe_party_finance_provider_sel"

    ' Add Finance Provider SQL
    Public Const ACAddStored As Boolean = True
    Public Const ACAddName As String = "AddPartyFinanceProvider"
    Public Const ACAddSQL As String = "spe_party_finance_provider_add"

    ' Delete Finance Provider SQL
    Public Const ACDeleteStored As Boolean = True
    Public Const ACDeleteName As String = "DeletePartyFinanceProvider"
    Public Const ACDeleteSQL As String = "spe_party_finance_provider_del"

    ' Update Finance Provider SQL
    Public Const ACUpdateStored As Boolean = True
    Public Const ACUpdateName As String = "UpdatePartyFinanceProvider"
    Public Const ACUpdateSQL As String = "spe_party_finance_provider_upd"
    'Ends
End Module