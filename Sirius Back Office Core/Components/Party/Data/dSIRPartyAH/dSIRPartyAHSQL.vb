Option Strict Off
Option Explicit On
Module SQL
    ' ***************************************************************** '
    ' Class Name: SQL
    '
    ' Date: 11/08/1999
    '
    ' Description: Contains the SQL Statements required by the
    '              SIRPartyAH class.
    '
    ' Edit History:
    ' ***************************************************************** '

    'SQL Statements

    ' Example select using embedded SQL
    ' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
    ' Public Const ACSelectStored = False
    ' Public Const ACSelectName = "SelectRisk"
    ' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
    'EK 12/10/99
    ' Select SIRPartyAH SQL
    Public Const ACSelectSingleStoredAH As Boolean = True
    Public Const ACSelectSingleNameAH As String = "SelectSingleSIRPartyAH"
    'developer guide no. 39
    Public Const ACSelectSingleSQLAH As String = "spe_party_account_handler_sel"

    ' Select SIRPartyCO SQL
    Public Const ACSelectSingleStoredCO As Boolean = True
    Public Const ACSelectSingleNameCO As String = "SelectSingleSIRPartyAH"
    'developer guide no. 39
    Public Const ACSelectSingleSQLCO As String = "spe_party_consultant_sel" ' Add SIRPartyAH SQL

    ' Select SIRPartyHC SQL
    Public Const ACSelectSingleStoredHC As Boolean = True
    Public Const ACSelectSingleNameHC As String = "SelectSingleSIRPartyHC"
    'developer guide no. 39
    Public Const ACSelectSingleSQLHC As String = "spe_party_executive_handler_sel"

    Public Const ACAddStored As Boolean = True

    'EK 12/10/99

    Public Const ACAddNameAH As String = "AddSIRPartyAH"
    'developer guide no. 39
    Public Const ACAddSQLAH As String = "spe_party_account_handler_add"

    ' SJP (CMG) PS235 030402003 Extra argument added (Commission_cnt)
    'developer guide no. 39
    Public Const ACAddNameCO As String = "AddSIRPartyCO"
    Public Const ACAddSQLCO As String = "spe_party_consultant_add"

    'DC260903 -PS256 -fsa compliance
    'AR20041202 PN17207 - pass commission cnt
    Public Const ACAddNameHC As String = "AddSIRPartyHC"
    'developer guide no. 39
    Public Const ACAddSQLHC As String = "spe_party_executive_handler_add"

    ' Delete SIRPartyAH SQL
    Public Const ACDeleteStoredAH As Boolean = True
    Public Const ACDeleteNameAH As String = "DeleteSIRPartyAH"
    'developer guide no. 39
    Public Const ACDeleteSQLAH As String = "spe_party_account_handler_del"

    'DC260903 -PS256 -fsa compliance
    ' Delete SIRPartyHC SQL
    Public Const ACDeleteStoredHC As Boolean = True
    Public Const ACDeleteNameHC As String = "DeleteSIRPartyAH"
    'developer guide no. 39
    Public Const ACDeleteSQLHC As String = "spe_party_executive_handler_del"

    ' Delete SIRPartyCO SQL
    Public Const ACDeleteStoredCO As Boolean = True
    Public Const ACDeleteNameCO As String = "DeleteSIRPartyCO"
    'developer guide no. 39
    Public Const ACDeleteSQLCO As String = "spe_party_consultant_del"

    ' Update SIRPartyAH SQL
    Public Const ACUpdateStoredAH As Boolean = True
    Public Const ACUpdateNameAH As String = "UpdateSIRPartyAH"
    'developer guide no. 39
    Public Const ACUpdateSQLAH As String = "spe_party_account_handler_upd"

    ' Update SIRPartyCO SQL
    ' SJP (CMG) PS235 030402003 Extra argument added (Commission_cnt)
    Public Const ACUpdateStoredCO As Boolean = True
    Public Const ACUpdateNameCO As String = "UpdateSIRPartyCO"
    'developer guide no. 39
    Public Const ACUpdateSQLCO As String = "spe_party_consultant_upd"

    ' Update SIRPartyHC SQL
    'AR20041202 PN17207 - pass commission cnt
    Public Const ACUpdateStoredHC As Boolean = True
    Public Const ACUpdateNameHC As String = "UpdateSIRPartyHC"
    'developer guide no. 39
    Public Const ACUpdateSQLHC As String = "spe_party_executive_handler_upd"
End Module