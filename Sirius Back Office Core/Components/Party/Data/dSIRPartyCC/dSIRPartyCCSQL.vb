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
    '              SIRPartyCC class.
    '
    ' Edit History:
    ' ***************************************************************** '

    'SQL Statements

    ' Example select using embedded SQL
    ' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
    ' Public Const ACSelectStored = False
    ' Public Const ACSelectName = "SelectRisk"
    ' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"

    ' Select SIRPartyCC SQL
    Public Const ACSelectSingleStored As Boolean = True
    Public Const ACSelectSingleName As String = "SelectSingleSIRPartyCC"
    Public Const ACSelectSingleSQL As String = "spe_Party_Corporate_Client_sel"

    ' Select SIRPartyCC from Event SQL
    Public Const ACSelectSingleEventStored As Boolean = True
    Public Const ACSelectSingleEventName As String = "SelectSingleSIREventPartyCC"
    Public Const ACSelectSingleEventSQL As String = "spu_Event_Party_CC_sel"

    ' Add SIRPartyCC SQL
    Public Const ACAddStored As Boolean = True
    Public Const ACAddName As String = "AddSIRPartyCC"
    Public Const ACAddSQL As String = "spe_Party_Corporate_Client_add"

    ' Delete SIRPartyCC SQL
    Public Const ACDeleteStored As Boolean = True
    Public Const ACDeleteName As String = "DeleteSIRPartyCC"
    Public Const ACDeleteSQL As String = "spe_Party_Corporate_Client_del"

    ' Update SIRPartyCC SQL
    Public Const ACUpdateStored As Boolean = True
    Public Const ACUpdateName As String = "UpdateSIRPartyCC"
    Public Const ACUpdateSQL As String = "spe_Party_Corporate_Client_upd"

    ' Copy PartyCC to event
    Public Const ACCopyPartyCCToEventStored As Boolean = True
    Public Const ACCopyPartyCCToEventName As String = "CopyPartyCCToEvent"
    Public Const ACCopyPartyCCToEventSQL As String = "spu_copy_party_cc_to_event"

    ' Copy Party to event
    Public Const ACCopyPartyToEventStored As Boolean = True
    Public Const ACCopyPartyToEventName As String = "CopyPartyToEvent"
    Public Const ACCopyPartyToEventSQL As String = "spu_copy_party_to_event"
End Module