Option Strict Off
Option Explicit On
Module ServicesSQL
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    '

    ' ***************************************************************** '
    ' Class Name: ServicesSQL
    '
    ' Date: 20/07/2000
    '
    ' Description: Contains the SQL Statements required by the
    '              bSIRParty.Services class.
    '
    ' Edit History: TF200700 - Created
    ' ***************************************************************** '

    ' RJG 09/06/00 Login PartyNC SQL
    Public Const ACLoginStored As Boolean = True
    Public Const ACLoginName As String = "LoginPartyNC"
    'Developer Guide No. 39 
    'Public Const ACLoginSQL As String = "{call spu_party_net_client_login_sel (?,?)}"
    Public Const ACLoginSQL As String = "spu_party_net_client_login_sel"
End Module