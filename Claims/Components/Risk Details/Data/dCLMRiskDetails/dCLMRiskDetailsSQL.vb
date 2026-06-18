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
    ' Date: {TodaysDate}
    '
    ' Description: Contains the SQL Statements required by the
    '              CLMRiskDetails class.
    '
    ' Edit History:
    ' ***************************************************************** '

    'SQL Statements

    ' Example select using embedded SQL
    ' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
    ' Public Const ACSelectStored = False
    ' Public Const ACSelectName = "SelectRisk"
    ' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
    'developer guide no.39
    'start
    ' Select CLMRiskDetails SQL
    Public Const ACSelectSingleStored As Boolean = True
    Public Const ACSelectSingleName As String = "SelectSingleCLMRiskDetails"
    Public Const ACSelectSingleSQL As String = "spe_{SQLTableName}_sel"

    ' Add CLMRiskDetails SQL
    Public Const ACAddStored As Boolean = True
    Public Const ACAddName As String = "AddCLMRiskDetails"
    Public Const ACAddSQL As String = "spe_{SQLTableName}_add"

    ' Delete CLMRiskDetails SQL
    Public Const ACDeleteStored As Boolean = True
    Public Const ACDeleteName As String = "DeleteCLMRiskDetails"
    Public Const ACDeleteSQL As String = "spe_{SQLTableName}_del"

    ' Update CLMRiskDetails SQL
    Public Const ACUpdateStored As Boolean = True
    Public Const ACUpdateName As String = "UpdateCLMRiskDetails"
    Public Const ACUpdateSQL As String = "spe_{SQLTableName}_upd"
End Module