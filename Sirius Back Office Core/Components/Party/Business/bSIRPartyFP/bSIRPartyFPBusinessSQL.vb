Option Strict Off
Option Explicit On
Module BusinessSQL
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    '

    ' ***************************************************************** '
    ' Class Name: BusinessSQL
    '
    ' Date: 16/10/2000
    '
    ' Description: Contains the SQL Statements required by the
    '              bSIRPartyFP.Business class.
    '
    ' Edit History: TF161000 - Created
    ' ***************************************************************** '

    'SQL Statements

    ' Select SIRPartyFP SQL
    Public Const ACSelectSingleStored As Boolean = True
    Public Const ACSelectSingleName As String = "SelectAllSIRPartyFP"
    'Developer Guide No 39
    Public Const ACSelectSingleSQL As String = "spe_party_finance_provider_sel"

    ' Add SIRPartyFP SQL
    Public Const ACAddStored As Boolean = True
    Public Const ACAddName As String = "AddSIRPartyFP"
    'Developer Guide No 39
    Public Const ACAddSQL As String = "spe_party_finance_provider_add"

    ' Edit SIRPartyFP SQL
    Public Const ACEditStored As Boolean = True
    Public Const ACEditName As String = "EditSIRPartyFP"
    'Developer Guide No 39
    Public Const ACEditSQL As String = "spe_party_finance_provider_upd"

    ' Delete SIRPartyFP SQL
    Public Const ACDeleteStored As Boolean = True
    Public Const ACDeleteName As String = "DeleteSIRPartyFP"
    'Developer Guide No 39
    Public Const ACDeleteSQL As String = "spe_party_finance_provider_del"
End Module