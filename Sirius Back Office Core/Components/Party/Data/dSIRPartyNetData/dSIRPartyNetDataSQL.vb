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
    ' Date: 20/07/2000
    '
    ' Description: Contains the SQL Statements required by the
    '              SIRPartyNetData class.
    '
    ' Edit History:
    ' ***************************************************************** '

    'SQL Statements

    ' Example select using embedded SQL
    ' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
    ' Public Const ACSelectStored = False
    ' Public Const ACSelectName = "SelectRisk"
    ' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"

    ' Select SIRPartyNetData SQL
    'developer guide no. 39
    'start
    Public Const ACSelectSingleStored As Boolean = True
    Public Const ACSelectSingleName As String = "SelectSingleSIRPartyNetData"
    Public Const ACSelectSingleSQL As String = "spe_Party_Net_Data_sel"

    ' Add SIRPartyNetData SQL
    Public Const ACAddStored As Boolean = True
    Public Const ACAddName As String = "AddSIRPartyNetData"
    Public Const ACAddSQL As String = "spe_Party_Net_Data_add"

    ' Delete SIRPartyNetData SQL
    Public Const ACDeleteStored As Boolean = True
    Public Const ACDeleteName As String = "DeleteSIRPartyNetData"
    Public Const ACDeleteSQL As String = "spe_Party_Net_Data_del"

    ' Update SIRPartyNetData SQL
    Public Const ACUpdateStored As Boolean = True
    Public Const ACUpdateName As String = "UpdateSIRPartyNetData"
    Public Const ACUpdateSQL As String = "spe_Party_Net_Data_upd"
    'end
End Module