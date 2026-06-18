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
    ' Date: 22/09/00
    '
    ' Description: Contains the SQL Statements required by the
    '              bSIRAddress.Business class.
    '
    ' Edit History:
    ' ***************************************************************** '

    'SQL Statements

    ' Example select using embedded SQL
    ' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
    ' Public Const ACSelectStored = False
    ' Public Const ACSelectName = "SelectRisk"
    ' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"

    Public Const ACSelRiskName As String = "SelectRisk"
    Public Const ACSelRiskStored As Boolean = False
    Public Const ACSelRiskSQL As String = "SELECT risk_cnt FROM insurance_file_risk_link" & Strings.ChrW(13) & Strings.ChrW(10) &
                                          "WHERE insurance_file_cnt = {insurance_file_cnt}" & Strings.ChrW(13) & Strings.ChrW(10) &
                                          "AND status_flag <> 'D'"
End Module