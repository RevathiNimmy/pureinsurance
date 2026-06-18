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
    ' Date: 08/07/02
    '
    ' Description: Contains the SQL Statements required by the
    '              SIRPartyAGG class. Created from dSIRPartyAG.
    '
    ' Edit History:
    ' ***************************************************************** '

    'SQL Statements

    ' Example select using embedded SQL
    ' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
    ' Public Const ACSelectStored = False
    ' Public Const ACSelectName = "SelectRisk"
    ' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"

    ' Select SIRPartyAGG SQL
    'Developer Guide no: 39
    Public Const ACSelectSingleStored As Boolean = True
    Public Const ACSelectSingleName As String = "SelectSingleSIRPartyAGG"
    Public Const ACSelectSingleSQL As String = "spe_Party_Agent_Group_sel"

    ' Add SIRPartyAGG SQL
    Public Const ACAddStored As Boolean = True
    Public Const ACAddName As String = "AddSIRPartyAGG"
    Public Const ACAddSQL As String = "spe_Party_Agent_Group_add"

    ' Delete SIRPartyAGG SQL
    Public Const ACDeleteStored As Boolean = True
    Public Const ACDeleteName As String = "DeleteSIRPartyAGG"
    Public Const ACDeleteSQL As String = "spe_Party_Agent_Group_del"

    ' Update SIRPartyAGG SQL
    Public Const ACUpdateStored As Boolean = True
    Public Const ACUpdateName As String = "UpdateSIRPartyAGG"
    Public Const ACUpdateSQL As String = "spe_Party_Agent_Group_upd"
End Module