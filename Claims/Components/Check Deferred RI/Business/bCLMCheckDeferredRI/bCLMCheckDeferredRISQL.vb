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
    ' Date: 02 Sept 2003
    '
    ' Description: Contains the SQL Statements required by the
    '              bCLMCheckDeferredRI.Business class.
    '
    ' Edit History:
    ' ***************************************************************** '

    'SQL Statements
    ' Example select using embedded SQL
    ' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
    ' Public Const ACSelectStored = False
    ' Public Const ACSelectName = "SelectRisk"
    ' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"

    ' AMB 02/09/2003: 1.8.6 Deferred Reinsurance development - get risk status for claim
    Public Const ACGetClaimRiskStatusStored As Boolean = True
    Public Const ACGetClaimRiskStatus As String = "GetClaimRiskStatus"
    'developer guide no.39
    Public Const ACGetClaimRiskStatusSQL As String = "spu_CLM_deferred_ri_status_sel"
End Module