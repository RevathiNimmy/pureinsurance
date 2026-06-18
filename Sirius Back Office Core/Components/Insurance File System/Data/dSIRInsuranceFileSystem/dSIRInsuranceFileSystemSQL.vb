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
    '              SIRInsuranceFileSystem class.
    '
    ' Edit History:
    ' ***************************************************************** '

    'SQL Statements

    ' Example select using embedded SQL
    ' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
    ' Public Const ACSelectStored = False
    ' Public Const ACSelectName = "SelectRisk"
    ' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"

    ' Select SIRInsuranceFileSystem SQL
    Public Const ACSelectSingleStored As Boolean = True
    Public Const ACSelectSingleName As String = "SelectSingleSIRInsuranceFileSystem"
    'developer guide no. 39
    Public Const ACSelectSingleSQL As String = "spe_Insurance_File_System_sel"

    ' Select SIRInsuranceFileSystem from Event SQL
    Public Const ACSelectSingleEventStored As Boolean = True
    Public Const ACSelectSingleEventName As String = "SelectSingleSIREventInsuranceFileSystem"
    'developer guide no. 39
    Public Const ACSelectSingleEventSQL As String = "spu_Event_Insurance_System_sel"

    ' Add SIRInsuranceFileSystem SQL
    Public Const ACAddStored As Boolean = True
    Public Const ACAddName As String = "AddSIRInsuranceFileSystem"
    'developer guide no. 39
    Public Const ACAddSQL As String = "spe_Insurance_File_System_add"

    ' Delete SIRInsuranceFileSystem SQL
    Public Const ACDeleteStored As Boolean = True
    Public Const ACDeleteName As String = "DeleteSIRInsuranceFileSystem"
    'developer guide no. 39
    Public Const ACDeleteSQL As String = "spe_Insurance_File_System_del"

    ' Update SIRInsuranceFileSystem SQL
    Public Const ACUpdateStored As Boolean = True
    Public Const ACUpdateName As String = "UpdateSIRInsuranceFileSystem"
    'developer guide no. 39
    Public Const ACUpdateSQL As String = "spe_Insurance_File_System_upd"

    'EK 05/09/99 Update Editable Event
    ' Update SIRInsuranceFileSystem SQL
    Public Const ACUpdateEventStored As Boolean = True
    Public Const ACUpdateEventName As String = "UpdateSIRInsuranceFileSystem"
    'developer guide no. 39
    Public Const ACUpdateEventSQL As String = "spe_Event_Ins_File_System_upd"
End Module