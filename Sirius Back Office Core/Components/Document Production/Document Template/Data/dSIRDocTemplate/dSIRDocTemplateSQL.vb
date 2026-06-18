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
    ' Date: 07/05/1999
    '
    ' Description: Contains the SQL Statements required by the
    '              SIREvent class.
    '
    ' Edit History:
    ' RKS 02/05/2005 Added document_filter to spe_document_template_add,
    '                spe_document_template_sel, spe_document_template_upd
    ' ***************************************************************** '

    'SQL Statements

    ' Example select using embedded SQL
    ' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
    ' Public Const ACSelectStored = False
    ' Public Const ACSelectName = "SelectRisk"
    ' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"

    ' Select SIREvent SQL
    'developer guide no.39
    Public Const ACSelectSingleStored As Boolean = True
    Public Const ACSelectSingleName As String = "SelectSingleSIRDocumentTemplate"
    Public Const ACSelectSingleSQL As String = "spe_document_template_sel"

    ' Add SIREvent SQL
    Public Const ACAddStored As Boolean = True
    Public Const ACAddName As String = "AddSIRDocumentTemplate"
    Public Const ACAddSQL As String = "spe_document_template_add"

    ' Delete SIREvent SQL
    Public Const ACDeleteStored As Boolean = True
    Public Const ACDeleteName As String = "DeleteSIRDocumentTemplate"
    Public Const ACDeleteSQL As String = "spe_document_template_del"

    ' Update SIREvent SQL
    Public Const ACUpdateStored As Boolean = True
    Public Const ACUpdateName As String = "UpdateSIRDocumentTemplate"
    Public Const ACUpdateSQL As String = "spe_document_template_upd"
End Module