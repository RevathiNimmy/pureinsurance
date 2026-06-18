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
    ' Date: 04/09/1998
    '
    ' Description: Contains the SQL Statements required by the
    '              SIRPartyAG class.
    '
    ' Edit History:
    ' ***************************************************************** '

    'SQL Statements

    ' Example select using embedded SQL
    ' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
    ' Public Const ACSelectStored = False
    ' Public Const ACSelectName = "SelectRisk"
    ' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"

    ' Select SIRPartyAG SQL
    Public Const ACSelectSingleStored As Boolean = True
    Public Const ACSelectSingleName As String = "SelectSingleSIRPartyAG"
    'Developer Guide No 39
    Public Const ACSelectSingleSQL As String = "spe_Party_Agent_sel"

    ' Add SIRPartyAG SQL
    'DC220803 -PS253 -fsa compliance
    'DC021203 -PN8727 -fsa compliance -registration number
    'DC141203 -added expense_account_id
    Public Const ACAddStored As Boolean = True
    Public Const ACAddName As String = "AddSIRPartyAG"
    Public Const ACAddSQL As String = "spe_Party_Agent_add"

    ' Delete SIRPartyAG SQL
    Public Const ACDeleteStored As Boolean = True
    Public Const ACDeleteName As String = "DeleteSIRPartyAG"

    'Developer Guide No 39
    Public Const ACDeleteSQL As String = "spe_Party_Agent_del"

    ' Update SIRPartyAG SQL
    'DC220803 -PS253 -fsa compliance
    'DC021203 -PN8727 -fsa compliance -registration number
    'DC141203 -added expense_account_id
    Public Const ACUpdateStored As Boolean = True
    Public Const ACUpdateName As String = "UpdateSIRPartyAG"
    Public Const ACUpdateSQL As String = "spe_Party_Agent_upd"
End Module