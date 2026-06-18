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
    ' Created: PW301002
    '
    ' Description: Contains the SQL Statements required by the
    '              bPMUSourceDefaults.Business class.
    '
    ' Edit History:
    ' ***************************************************************** '

    'SQL Statements
    Public Const ACGetBranchDefaultsStored As Boolean = True
    Public Const ACGetBranchDefaultsName As String = "Get Branch Defaults"
    Public Const ACGetBranchDefaultsSQL As String = "spu_get_branch_defaults"

    Public Const ACSaveBranchDefaultsStored As Boolean = True
    Public Const ACSaveBranchDefaultsName As String = "Save Branch Defaults"
    Public Const ACSaveBranchDefaultsSQL As String = "spu_save_branch_defaults"

    Public Const ACGetBranchAgentsStored As Boolean = True
    Public Const ACGetBranchAgentsName As String = "Get Branch Agents"
    Public Const ACGetBranchAgentsSQL As String = "spu_Get_BranchAgents"
End Module