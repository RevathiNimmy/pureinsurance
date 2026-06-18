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
    ' Date: 17/07/02
    '
    ' Description: Contains the SQL Statements required by the
    '              bSIRGetDocument.Business class.
    '
    ' Edit History:
    ' ***************************************************************** '

    'SQL Statements

    ' Select Insurance_File record
    Public Const ACGetInsuranceFileStored As Boolean = True
    Public Const ACGetInsuranceFileName As String = "SelectInsuranceFile"
    'Developer Guide No 39
    Public Const ACGetInsuranceFileSQL As String = "spe_insurance_file_sel"

    ' select suppressed docs from party_agent
    Public Const ACGetSuppressedDocsStored As Boolean = True
    Public Const ACGetSuppressedDocsName As String = "SelectSuppressedDocs"
    'Developer Guide No 39
    Public Const ACGetSuppressedDocsSQL As String = "spu_get_agent_docs"
End Module