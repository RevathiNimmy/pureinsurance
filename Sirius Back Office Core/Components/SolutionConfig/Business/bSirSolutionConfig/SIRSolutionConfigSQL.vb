Option Strict Off
Option Explicit On
Module SIRSolutionConfigSQL
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    '


    ' ***************************************************************** '
    ' Class Name: SIRSolutionConfigSQL
    '
    ' Date: 28th July 1999
    '
    ' Description: Contains SQL Statements (Stored Procedures
    '              and Embedded SQL)
    '
    ' Edit History:
    ' ***************************************************************** '


    'SQL Statements
    Public Const ACSelectPolicyStored As Boolean = False
    Public Const ACSelectPolicyName As String = "SelectPolicy"
    Public Const ACSelectPolicy As String = "SELECT " &
                                            "policy_key0, policy_folder_key, policy_no, insurer_no, " &
                                            "company_no, policy_ver, policy_status, status_ind, " &
                                            "effective_date, date_of_update, record_status, invariant_key, " &
                                            "scheme_no, scheme_ver " &
                                            "FROM Policy WHERE policy_key0 = {policy_key0}"
End Module