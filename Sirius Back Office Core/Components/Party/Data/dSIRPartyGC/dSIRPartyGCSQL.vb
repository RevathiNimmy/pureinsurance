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
    '              SIRPartyGC class.
    '
    ' Edit History:
    ' ***************************************************************** '

    'SQL Statements

    ' Example select using embedded SQL
    ' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
    ' Public Const ACSelectStored = False
    ' Public Const ACSelectName = "SelectRisk"
    ' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"

    ' Select SIRPartyGC SQL
    Public Const ACSelectSingleStored As Boolean = True
    Public Const ACSelectSingleName As String = "SelectSingleSIRPartyGC"
    'developer guide no. 39(GUIDE)
    Public Const ACSelectSingleSQL As String = "spe_Party_Group_Client_sel"

    ' Select SIRPartyGC from Event SQL
    Public Const ACSelectSingleEventStored As Boolean = True
    Public Const ACSelectSingleEventName As String = "SelectSingleSIREventPartyGC"
    'developer guide no. 39(GUIDE)
    Public Const ACSelectSingleEventSQL As String = "spu_Event_Party_GC_sel"

    ' Add SIRPartyGC SQL
    'mkw090204 PN10359. Add TPS, Emps and Mailshot fields.
    Public Const ACAddStored As Boolean = True
    Public Const ACAddName As String = "AddSIRPartyGC"
    'developer guide no. 39(GUIDE)
    Public Const ACAddSQL As String = "spe_Party_Group_Client_add"

    ' Delete SIRPartyGC SQL
    Public Const ACDeleteStored As Boolean = True
    Public Const ACDeleteName As String = "DeleteSIRPartyGC"
    'developer guide no. 39(GUIDE)
    Public Const ACDeleteSQL As String = "spe_Party_Group_Client_del"

    ' Update SIRPartyGC SQL
    Public Const ACUpdateStored As Boolean = True
    Public Const ACUpdateName As String = "UpdateSIRPartyGC"
    'mkw090204 PN10359. Add TPS, Emps and Mailshot fields.
    'developer guide no. 39(GUIDE)
    Public Const ACUpdateSQL As String = "spe_Party_Group_Client_upd"

    ' Copy PartyGC to event
    Public Const ACCopyPartyGCToEventStored As Boolean = True
    Public Const ACCopyPartyGCToEventName As String = "CopyPartyGCToEvent"
    'developer guide no. 39(GUIDE)
    Public Const ACCopyPartyGCToEventSQL As String = "spu_copy_party_gc_to_event"

    ' Copy Party to event
    Public Const ACCopyPartyToEventStored As Boolean = True
    Public Const ACCopyPartyToEventName As String = "CopyPartyToEvent"
    'developer guide no. 39(GUIDE)
    Public Const ACCopyPartyToEventSQL As String = "spu_copy_party_to_event"
End Module