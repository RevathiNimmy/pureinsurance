Option Strict Off
Option Explicit On
Module SQL
    ' ***************************************************************** '
    ' Class Name: SQL
    '
    ' Date: 04/09/1998
    '
    ' Description: Contains the SQL Statements required by the
    '              SIRPartyPC class.
    '
    ' Edit History:
    ' ***************************************************************** '

    'SQL Statements

    ' Example select using embedded SQL
    ' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
    ' Public Const ACSelectStored = False
    ' Public Const ACSelectName = "SelectRisk"
    ' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"

    ' Select SIRPartyPC SQL
    Public Const ACSelectSingleStored As Boolean = True
    Public Const ACSelectSingleName As String = "SelectSingleSIRPartyPC"
    Public Const ACSelectSingleSQL As String = "spe_Party_Personal_Client_sel"

    ' Select SIRPartyPC from event SQL
    Public Const ACSelectSingleEventStored As Boolean = True
    Public Const ACSelectSingleEventName As String = "SelectSingleSIREventPartyPC"
    Public Const ACSelectSingleEventSQL As String = "spu_event_Party_PC_sel"

    ' Add SIRPartyPCLifestyle SQL
    Public Const ACAddLifestyleStored As Boolean = True
    Public Const ACAddLifestyleName As String = "AddSIRPartyPCLifestyle"
    Public Const ACAddLifestyleSQL As String = "spe_Party_Lifestyle_add"

    ' Add SIRPartyPC SQL
    Public Const ACAddStored As Boolean = True
    Public Const ACAddName As String = "AddSIRPartyPC"
    Public Const ACAddSQL As String = "spe_Party_Personal_Client_add"

    ' Delete SIRPartyPC SQL
    Public Const ACDeleteStored As Boolean = True
    Public Const ACDeleteName As String = "DeleteSIRPartyPC"
    Public Const ACDeleteSQL As String = "spe_Party_Personal_Client_del"

    ' Update SIRPartyPC SQL
    Public Const ACUpdateStored As Boolean = True
    Public Const ACUpdateName As String = "UpdateSIRPartyPC"
    Public Const ACUpdateSQL As String = "spe_Party_Personal_Client_upd"

    ' Update SIRPartyPCLifestyle SQL
    Public Const ACUpdateLifestyleStored As Boolean = True
    Public Const ACUpdateLifestyleName As String = "UpdateSIRPartyPCLifestyle"
    Public Const ACUpdateLifestyleSQL As String = "spe_Party_Lifestyle_upd"

    ' Copy PartyPC to event
    Public Const ACCopyPartyPCToEventStored As Boolean = True
    Public Const ACCopyPartyPCToEventName As String = "CopyPartyPCToEvent"
    Public Const ACCopyPartyPCToEventSQL As String = "spu_copy_party_pc_to_event"

    ' Copy Party to event
    Public Const ACCopyPartyToEventStored As Boolean = True
    Public Const ACCopyPartyToEventName As String = "CopyPartyToEvent"
    Public Const ACCopyPartyToEventSQL As String = "spu_copy_party_to_event"
End Module