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
    ' Date: 02/09/2000
    '
    ' Description: Contains the SQL Statements required by the
    '              bSIRRenInvitePrint.Business class.
    '
    ' Edit History:
    ' ***************************************************************** '

    Public Const ACGetClientPolicyDetailsStored As Boolean = True
    Public Const ACGetClientPolicyDetailsName As String = "GetClientPolicyDetails"
    Public Const ACGetClientPolicyDetailsSQL As String = "{call spu_get_client_policy_details (?)}"
End Module