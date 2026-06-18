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
    ' Class Name    :BusinessSQL
    ' Date          :07/09/2000
    ' Description   :Contains the SQL Statements required by the
    '               bCLMRiskType.Business class.
    ' Edit History: Pandu
    ' ***************************************************************** '

    'SQL Statements

    ' Select All RiskTypes Underwriting SQL
    Public Const ACGetRiskTypesUnderwritingStored As Boolean = True
    Public Const ACGetRiskTypesUnderWritingName As String = "GetRiskTypesUnderWriting"
    Public Const ACGetRiskTypesUnderWritingSQL As String = "spu_get_risk_types_UW"

    ' Select All RiskTypes Broking SQL
    Public Const ACGetRiskTypesBrokingStored As Boolean = True
    Public Const ACGetRiskTypesBrokingName As String = "GetRiskTypesBroking"
    Public Const ACGetRiskTypesBrokingSQL As String = "spu_get_risk_types_BRK"

    ' Alix
    ' Select All GIS Screen
    Public Const ACGetGISScreensStored As Boolean = True
    Public Const ACGetGISScreensName As String = "GetGISScreens"
    Public Const ACGetGISScreensSQL As String = "spu_get_GIS_screens"

    ' Alix
    ' Update risk_type selected screen
    Public Const ACRiskTypeScreenUpdStored As Boolean = True
    Public Const ACRiskTypeScreenUpdName As String = "RiskTypeScreenUpd"
    Public Const ACRiskTypeScreenUpdSQL As String = "spu_Risk_Type_ClaimScreen_upd"
End Module