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
    ' Date: 25/06/1999
    '
    ' Description: Contains the SQL Statements required by the
    '              other party class.
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
    ' Select otherparty SQL
    Public Const ACSelectSingleStored As Boolean = True
    Public Const ACSelectSingleName As String = "SelectSingleotherparty"
    Public Const ACSelectSingleSQL As String = "spe_party_other_sel"

    ' Add otherparty SQL
    Public Const ACAddStored As Boolean = True
    Public Const ACAddName As String = "Addotherparty"
    Public Const ACAddSQL As String = "spe_party_other_add"

    ' Delete otherparty SQL
    Public Const ACDeleteStored As Boolean = True
    Public Const ACDeleteName As String = "Deleteotherparty"
    Public Const ACDeleteSQL As String = "spe_party_other_del"

    ' Update otherparty SQL
    Public Const ACUpdateStored As Boolean = True
    Public Const ACUpdateName As String = "Updateotherparty"
    Public Const ACUpdateSQL As String = "spe_party_other_upd"
    'end
End Module