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
    '              SIRPartyEX class.
    '
    ' Edit History:
    ' AMB 09-Oct-03: 1.8.6 Accident Management development - created
    ' ***************************************************************** '

    'SQL Statements

    ' Example select using embedded SQL
    ' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
    ' Public Const ACSelectStored = False
    ' Public Const ACSelectName = "SelectRisk"
    ' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"

    ' Select SIRPartyEX SQL
    Public Const ACSelectSingleStored As Boolean = True
    Public Const ACSelectSingleName As String = "SelectSingleSIRPartyEX"
    Public Const ACSelectSingleSQL As String = "spu_Party_Extra_sel"

    ' Add SIRPartyEX SQL
    Public Const ACAddStored As Boolean = True
    Public Const ACAddName As String = "AddSIRPartyEX"
    Public Const ACAddSQL As String = "spu_Party_Extra_ins"

    ' Delete SIRPartyEX SQL
    Public Const ACDeleteStored As Boolean = True
    Public Const ACDeleteName As String = "DeleteSIRPartyEX"
    Public Const ACDeleteSQL As String = "spu_Party_Extra_del"

    ' Update SIRPartyEX SQL
    Public Const ACUpdateStored As Boolean = True
    Public Const ACUpdateName As String = "UpdateSIRPartyEX"
    Public Const ACUpdateSQL As String = "spu_Party_Extra_upd"
End Module