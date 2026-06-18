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
    ' Date: 18/09/1998
    '
    ' Description: Contains the SQL Statements required by the
    '              bSIRInsuranceFile.Services class.
    '
    ' Edit History:
    ' ***************************************************************** '

    'SQL Statements

    ' Example select using embedded SQL
    ' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
    ' Public Const ACSelectStored = False
    ' Public Const ACSelectName = "SelectRisk"
    ' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"

    ' Select All SIRInsuranceFile SQL
    Public Const ACGetAllEntityDetailsStored As Boolean = True
    Public Const ACGetAllEntityDetailsName As String = "SelectAllSIRInsuranceFileEntity"
    'Developer Guide No 39. 
    Public Const ACGetAllEntityDetailsSQL As String = "spu_Insurance_File_Entity_sel"

    ' Select Party Name From ID SQL
    Public Const ACGetPartyNameFromIDStored As Boolean = False
    Public Const ACGetPartyNameFromIDName As String = "GetPartyNameFromID"
    'Developer Guide No 39. 
    Public Const ACGetPartyNameFromIDSQL As String = ""

    ' Select InsuranceFileCnt From ID SQL
    Public Const ACGetInsFileCntFromIDStored As Boolean = False
    Public Const ACGetInsFileCntFromIDName As String = "ACGetInsFileCntFromID"
    'Developer Guide No 39.
    Public Const ACGetInsFileCntFromIDSQL As String = ""

    ' Select Party Data From ABI SQL
    Public Const ACGetPartyDataFromABIStored As Boolean = False
    Public Const ACGetPartyDataFromABIName As String = "GetPartyDataFromABI"
    'Developer Guide No 39.
    Public Const ACGetPartyDataFromABISQL As String = ""

    'sj 06/09/2002 - start
    Public Const ACCopyPolicyAgentsStored As Boolean = True
    Public Const ACCopyPolicyAgentsName As String = "CopyPolicyAgents"
    'Developer Guide No 39.
    Public Const ACCopyPolicyAgentsSQL As String = "spu_copy_policy_agents"
    'sj 06/09/2002 - end

    ' CJB 100804 PN13723
    Public Const ACGetPolicySubAgentStored As Boolean = True
    Public Const ACGetPolicySubAgentName As String = "GetPolicySubAgent"
    'Developer Guide No 39.
    Public Const ACGetPolicySubAgentSQL As String = "spu_Policy_Sub_Agent_sel"

    Public Const kGetDoNotMergeClausesStored As Boolean = True
    Public Const kGetDoNotMergeClausesName As String = "GetDoNotMergeClauses"
    Public Const kGetDoNotMergeClausesSQL As String = "spu_Get_Do_Not_Merge_Clauses"
End Module