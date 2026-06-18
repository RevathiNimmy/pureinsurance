Option Strict Off
Option Explicit On
Imports System
Module bGISQEMCOMPIILEDSQL
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    '

    ' ***************************************************************** '
    ' Class Name: bGISQEMPMUSQL
    '
    ' Date: 6th December 2000
    '
    ' Description: Contains the SQL Statements to (Stored Procedures
    '              and Enbedded SQL) manipulate an bGISQEMPMUSQL
    '
    ' Edit History:
    ' ***************************************************************** '

    'SQL Statements

    ' Get Rule File Name SQL
    Public Const ACGetRuleFileNameStored As Boolean = True
    Public Const ACGetRuleFileNameName As String = "GetRuleFileName"
    Public Const ACGetRuleFileNameSQL As String = "spu_get_rule_file_name"

    ' Get UAL Rule File Name SQL
    Public Const ACGetUALRuleFileNameStored As Boolean = True
    Public Const ACGetUALRuleFileNameName As String = "GetUALRuleFileName"
    Public Const ACGetUALRuleFileNameSQL As String = "spu_get_ual_rule_file_name"

    ' Get Screen Code SQL
    Public Const ACGetScreenCodeStored As Boolean = True
    Public Const ACGetScreenCodeName As String = "GetScreenCode"
    Public Const ACGetScreenCodeSQL As String = "spu_get_gis_screen_code"

    ' RAW 15/11/2004 : Pricing Changes : added
    Public Const ACGetRiskTypeRuleSetStored As Boolean = True
    Public Const ACGetRiskTypeRuleSetName As String = "GetRiskTypeRuleSet"
    Public Const ACGetRiskTypeRuleSetSQL As String = "spe_risk_type_rule_set_sel"

    ' RAW 15/11/2004 : Pricing Changes : added
    Public Const ACUpdateRiskRuleDetailsStored As Boolean = True
    Public Const ACUpdateRiskRuleDetailsName As String = "UpdateRiskRuleDetails"
    Public Const ACUpdateRiskRuleDetailsSQL As String = "spu_update_risk_rule_details"

End Module