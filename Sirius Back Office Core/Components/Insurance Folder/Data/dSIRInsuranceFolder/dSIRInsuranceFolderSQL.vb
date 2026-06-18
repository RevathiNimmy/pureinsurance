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
    ' Date: 08/09/1998
    '
    ' Description: Contains the SQL Statements required by the
    '              SIRInsuranceFolder class.
    '
    ' Edit History:
    ' ***************************************************************** '

    'SQL Statements

    ' Example select using embedded SQL
    ' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
    ' Public Const ACSelectStored = False
    ' Public Const ACSelectName = "SelectRisk"
    ' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"

    ' Select SIRInsuranceFolder SQL
    Public Const ACSelectSingleStored As Boolean = True
    Public Const ACSelectSingleName As String = "SelectSingleSIRInsuranceFolder"
    Public Const ACSelectSingleSQL As String = "spe_Insurance_Folder_sel"

    ' Select SIRInsuranceFolder from event SQL
    Public Const ACSelectSingleEventStored As Boolean = True
    Public Const ACSelectSingleEventName As String = "SelectSingleSIREventInsuranceFolder"
    Public Const ACSelectSingleEventSQL As String = "spe_Event_Insurance_Folder_sel"

    ' Add SIRInsuranceFolder SQL
    Public Const ACAddStored As Boolean = True
    Public Const ACAddName As String = "AddSIRInsuranceFolder"
    Public Const ACAddSQL As String = "spe_Insurance_Folder_add"

    ' Delete SIRInsuranceFolder SQL
    Public Const ACDeleteStored As Boolean = True
    Public Const ACDeleteName As String = "DeleteSIRInsuranceFolder"
    Public Const ACDeleteSQL As String = "spe_Insurance_Folder_del"

    ' Update SIRInsuranceFolder SQL
    Public Const ACUpdateStored As Boolean = True
    Public Const ACUpdateName As String = "UpdateSIRInsuranceFolder"
    Public Const ACUpdateSQL As String = "spe_Insurance_Folder_upd"

    'EK 05/09/99 Update Editable Event
    Public Const ACUpdateEventStored As Boolean = True
    Public Const ACUpdateEventName As String = "UpdateSIRInsuranceFolder"
    Public Const ACUpdateEventSQL As String = "spe_Event_Ins_Folder_upd"

    'EK 05/09/99 Update Editable Event
    Public Const ACDuplicatePolicyFixStored As Boolean = True
    Public Const ACDuplicatePolicyFixName As String = "DuplicatePolicyFixFolder"
    'developer guide no. 39
    Public Const ACDuplicatePolicyFixSQL As String = "spe_duplicate_policy_fix"
End Module