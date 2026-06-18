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
    ' Date: 25/06/1999
    '
    ' Description: Contains the SQL Statements required by the
    '              PartyNC class.
    '
    ' Edit History:
    ' ***************************************************************** '

    'SQL Statements

    ' Example select using embedded SQL
    ' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
    ' Public Const ACSelectStored = False
    ' Public Const ACSelectName = "SelectRisk"
    ' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"

    ' Select PartyNC SQL
    Public Const ACSelectSingleStored As Boolean = True
    Public Const ACSelectSingleName As String = "SelectSinglePartyNC"
    'developer guide no. 36
    Public Const ACSelectSingleSQL As String = "spe_party_net_client_sel"

    ' Add PartyNC SQL
    Public Const ACAddStored As Boolean = True
    Public Const ACAddName As String = "AddPartyNC"
    'developer guide no. 36
    Public Const ACAddSQL As String = "spe_party_net_client_add"

    ' Delete PartyNC SQL
    Public Const ACDeleteStored As Boolean = True
    Public Const ACDeleteName As String = "DeletePartyNC"
    'developer guide no. 36
    Public Const ACDeleteSQL As String = "spe_party_net_client_del"

    ' Update PartyNC SQL
    Public Const ACUpdateStored As Boolean = True
    Public Const ACUpdateName As String = "UpdatePartyNC"
    'developer guide no. 36
    Public Const ACUpdateSQL As String = "spe_party_net_client_upd"

    ' RJG 09/06/00 Login PartyNC SQL
    Public Const ACLoginStored As Boolean = True
    Public Const ACLoginName As String = "LoginPartyNC"
    'developer guide no. 36
    Public Const ACLoginSQL As String = "spe_party_net_client_login_sel"

    'eck060600

    ' Select SIRPartyLifestyle SQL
    Public Const ACSelectLifestyleStored As Boolean = True
    Public Const ACSelectLifestyleName As String = "SelectSIRPartyLifestyle"
    'developer guide no. 36
    Public Const ACSelectLifestyleSQL As String = "spe_Party_Lifestyle_sel"

    ' Add SIRPartyLifestyle SQL
    Public Const ACAddLifestyleStored As Boolean = True
    Public Const ACAddLifestyleName As String = "AddSIRPartyLifestyle"
    'developer guide no. 36
    Public Const ACAddLifestyleSQL As String = "spe_Party_Lifestyle_add"

    ' Update SIRPartyLifestyle SQL
    Public Const ACUpdateLifestyleStored As Boolean = True
    Public Const ACUpdateLifestyleName As String = "UpdateSIRPartyLifestyle"
    'developer guide no. 36
    Public Const ACUpdateLifestyleSQL As String = "spe_Party_Lifestyle_upd"
End Module